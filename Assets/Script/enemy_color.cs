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
        float timeout = 5f; // 최대 허용 시간 (초)
        EnemyTurnCompleted = false;

        yield return StartCoroutine(EnemyTurnLogic());

        float elapsed = 0f;
        while (!EnemyTurnCompleted && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!EnemyTurnCompleted)
        {
            Debug.LogWarning($"{name}의 턴이 제한 시간을 초과하여 강제 종료됩니다.");
            Turn.Turn_next(this);
        }
    }

    private IEnumerator EnemyTurnLogic()
    {
        yield return new WaitForSeconds(0.5f);  // 턴 시작 딜레이

        System.Random rnd = new System.Random();
        int skill_index = rnd.Next(skills.Count); 
        monoskill selectedSkill = every_skill.get_skill(skills[skill_index]);

        // 공격 대상 수집
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(new Vector2(0, 0), new Vector2(18, 18), LayerMask.GetMask("Default"));
        List<my_color> my_colors = new List<my_color>();
        foreach (Collider2D col in hitColliders)
        {
            if (col.GetComponent<my_color>() != null)
                my_colors.Add(col.GetComponent<my_color>());
        }

        if (my_colors.Count == 0)
        {
            Turn.Turn_next(this);
            EnemyTurnCompleted = true;
            yield break;
        }

        int target_index = rnd.Next(my_colors.Count);
        y_color target = my_colors[target_index];
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;

        if (selectedSkill.skill_availablity(this, target))
        {
            yield return StartCoroutine(UseSkillRoutine(target, selectedSkill));
            Turn.Turn_next(this);
            EnemyTurnCompleted = true;
            yield break;
        }

        // 이동 후보 계산
        List<Vector3> candidates = new List<Vector3>();
        int maxStep = distance;
        for (int dx = -maxStep; dx <= maxStep; dx++)
        {
            for (int dy = -maxStep; dy <= maxStep; dy++)
            {
                int cost = Mathf.Abs(dx) + Mathf.Abs(dy);
                if (cost > maxStep) continue;

                Vector3 point = new Vector3(Mathf.Round(startPos.x + dx), Mathf.Round(startPos.y + dy), 0f);
                Collider2D[] overlaps = Physics2D.OverlapCircleAll(point, 0.3f, LayerMask.GetMask("Default"));
                bool blocked = false;
                foreach (var col in overlaps)
                {
                    if (col.GetComponent<y_color>() != null && col.gameObject != this.gameObject)
                    {
                        blocked = true;
                        break;
                    }
                }
                if (!blocked)
                    candidates.Add(point);
            }
        }

        // 후보 없을 경우 fallback
        if (candidates.Count == 0)
        {
            List<Vector3> randomFallbacks = new List<Vector3>();

            for (int x = 0; x <= 18; x++)
            {
                for (int y = 0; y <= 18; y++)
                {
                    Vector3 point = new Vector3(x, y, 0f);

                    float manhattan = Mathf.Abs(point.x - startPos.x) + Mathf.Abs(point.y - startPos.y);
                    if (manhattan > distance) continue;

                    Collider2D[] overlaps = Physics2D.OverlapCircleAll(point, 0.3f, LayerMask.GetMask("Default"));
                    bool blocked = false;
                    foreach (var col in overlaps)
                    {
                        if (col.GetComponent<y_color>() != null && col.gameObject != this.gameObject)
                        {
                            blocked = true;
                            break;
                        }
                    }

                    if (!blocked)
                        randomFallbacks.Add(point);
                }
            }

            if (randomFallbacks.Count > 0)
            {
                Vector3 randomMove = randomFallbacks[UnityEngine.Random.Range(0, randomFallbacks.Count)];
                transform.position = randomMove;
                distance -= (int)Mathf.Round(Vector3.Distance(startPos, randomMove));
            }
        }

        if (candidates.Count > 0)
        {
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
        }

        Turn.Turn_next(this);
        EnemyTurnCompleted = true;
        yield break;
    }

    public void Update_skill(){
        color = render.color;
        skills.Add(color);
    }
}