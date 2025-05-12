using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
public class enemy_color : y_color
{
    // Start is called before the first frame update
    void Start(){
        render = GetComponent<SpriteRenderer>();
        color = render.color;
        Update_stat();
        Update_HP();
        Update_skill();
        start_button = GetComponent<start_button>();
    }

    public start_button start_button;
    void Update() {
        if (start_button.started && Turn.turn_order.Count > 0 && this == Turn.turn_order[0] && !skill_locked) {
            skill_locked = true;
            StartCoroutine(EnemyTurnRoutine());
        }
        if(this.transform.position.x < 0 || this.transform.position.x > 18 || this.transform.position.y < 0 || this.transform.position.y > 18){
            UnityEngine.Object.Destroy(this.gameObject);
            GameObject.FindObjectOfType<TurnUI>()?.UpdateTurnDisplay();
        }
    }

    void OnDestroy()
    {
        StageManager.Instance?.OnEnemyDefeated();
    }

    private bool EnemyTurnCompleted = false;

    public IEnumerator EnemyTurnRoutine()
    {
        float timeout = 10f;
        EnemyTurnCompleted = false;

        Coroutine logicCoroutine = StartCoroutine(EnemyTurnLogic());

        float elapsed = 0f;
        while (elapsed < timeout)
        {
            if (EnemyTurnCompleted)
            {
                Turn.Turn_next(this);
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Turn.Turn_next(this);
    }

    private IEnumerator EnemyTurnLogic(){
        yield return new WaitForSeconds(0.5f);  // 턴 시작 딜레이

        System.Random rnd = new System.Random();
        int skill_index = rnd.Next(skills.Count);
        monoskill selectedSkill = every_skill.get_skill(skills[skill_index]);

        // 1. 공격 대상 수집
        List<my_color> my_colors = new List<my_color>();
        foreach (var unit in GameObject.FindObjectsOfType<my_color>())
        {
            if (unit.stage_set == 1)
                my_colors.Add(unit);
        }

        if (my_colors.Count == 0)
        {
            EnemyTurnCompleted = true;
            yield break;
        }

        y_color target = my_colors[rnd.Next(my_colors.Count)];
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;

        // 2. 사거리 안이면 바로 공격
        if (selectedSkill.skill_availablity(this, target))
        {
            yield return StartCoroutine(UseSkillRoutine(target, selectedSkill));
            EnemyTurnCompleted = true;
            yield break;
        }

        // 3. 이동 후보 계산
        List<Vector3> candidates = new List<Vector3>();
        int maxStep = distance;

        for (int dx = -maxStep; dx <= maxStep; dx++)
        {
            for (int dy = -maxStep; dy <= maxStep; dy++)
            {
                int cost = Mathf.Abs(dx) + Mathf.Abs(dy);
                if (cost > maxStep) continue;

                Vector3 point = new Vector3(Mathf.Round(startPos.x + dx), Mathf.Round(startPos.y + dy), 0f);
                bool blocked = Physics2D.OverlapCircle(point, 0.3f, LayerMask.GetMask("Default"));
                if (!blocked)
                    candidates.Add(point);
            }
        }

        // 4. fallback 이동
        if (candidates.Count == 0)
        {
            List<Vector3> fallback = new List<Vector3>();
            for (int x = 0; x <= 18; x++)
            {
                for (int y = 0; y <= 18; y++)
                {
                    Vector3 point = new Vector3(x, y, 0f);
                    float manhattan = Mathf.Abs(point.x - startPos.x) + Mathf.Abs(point.y - startPos.y);
                    if (manhattan > distance) continue;

                    bool blocked = Physics2D.OverlapCircle(point, 0.3f, LayerMask.GetMask("Default"));
                    if (!blocked)
                        fallback.Add(point);
                }
            }

            if (fallback.Count > 0)
            {
                Vector3 move = fallback[UnityEngine.Random.Range(0, fallback.Count)];
                transform.position = move;
                distance -= (int)Mathf.Round(Vector3.Distance(startPos, move));
            }

            EnemyTurnCompleted = true;
            yield break;
        }

        // 5. 이동 후 다시 시도
        Vector3 best = candidates[0];
        float minDist = Vector3.Distance(best, targetPos);
        foreach (var pt in candidates)
        {
            float d = Vector3.Distance(pt, targetPos);
            if (d < minDist)
            {
                minDist = d;
                best = pt;
            }
        }

        transform.position = best;
        distance -= (int)Mathf.Round(Vector3.Distance(startPos, best));

        if (selectedSkill.skill_availablity(this, target))
        {
            yield return StartCoroutine(UseSkillRoutine(target, selectedSkill));
        }

        // 6. 최종 종료
        EnemyTurnCompleted = true;
        yield break;
    }

    public void Update_skill(){
        color = render.color;
        skills.Add(color);
    }
}