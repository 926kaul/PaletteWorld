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

    IEnumerator EnemyTurnRoutine() {
        yield return new WaitForSeconds(0.5f);  // 턴 시작 딜레이 추가

        System.Random rnd = new System.Random();
        int skill_index = rnd.Next(skills.Count); //enemy가 가진 스킬 중 하나 랜덤으로 선택

        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(new Vector2(0, 0), new Vector2(18, 18), LayerMask.GetMask("Default"));
        List<my_color> my_colors = new List<my_color>();
        foreach (Collider2D collider in hitColliders) {
            if (collider.GetComponent<my_color>() != null) {
                my_colors.Add(collider.GetComponent<my_color>());
            }
        }

        if (my_colors.Count > 0) {
            int target_index = rnd.Next(my_colors.Count); //enemy가 공격할 my color 랜덤으로 선택
            monoskill enemy_selected_skill = every_skill.get_skill(skills[skill_index]);
            if (enemy_selected_skill.skill_availablity(this, my_colors[target_index])) {
                yield return StartCoroutine(UseSkillRoutine(my_colors[target_index], enemy_selected_skill));
                Turn.Turn_next(this);
                yield break;
            }
            else {
                y_color target = my_colors[target_index];
                Vector3 startPos = transform.position;
                Vector3 targetPos = target.transform.position;

                // 이동 가능한 범위 내 격자점 후보 수집
                List<Vector3> candidates = new List<Vector3>();
                int maxStep = distance;
                for (int dx = -maxStep; dx <= maxStep; dx++) {
                    for (int dy = -maxStep; dy <= maxStep; dy++) {
                        int cost = Mathf.Abs(dx) + Mathf.Abs(dy); // 맨해튼 거리
                        if (cost > maxStep) continue; // 이동 불가한 거리

                        Vector3 point = new Vector3(Mathf.Round(startPos.x + dx), Mathf.Round(startPos.y + dy), 0f);
                        // 충돌 체크
                        Collider2D[] overlaps = Physics2D.OverlapCircleAll(point, 0.3f, LayerMask.GetMask("Default"));
                        bool blocked = false;
                        foreach (var col in overlaps) {
                            if (col.GetComponent<y_color>() != null && col.gameObject != this.gameObject) {
                                blocked = true;
                                break;
                            }
                        }
                        if (!blocked) candidates.Add(point);
                    }
                }

                // 가장 가까운 후보를 선택
                if (candidates.Count > 0) {
                    Vector3 best = candidates[0];
                    float minDist = Vector3.Distance(best, targetPos);
                    foreach (var pt in candidates) {
                        float d = Vector3.Distance(pt, targetPos);
                        if (d < minDist) {
                            minDist = d;
                            best = pt;
                        }
                    }

                    // 실제 이동
                    transform.position = best;
                    distance -= (int)Mathf.Round(Vector3.Distance(startPos, best));

                    // 이동 후 다시 스킬 시도
                    if (enemy_selected_skill.skill_availablity(this, my_colors[target_index])) {
                        yield return StartCoroutine(UseSkillRoutine(my_colors[target_index], enemy_selected_skill));
                    }
                }

                Turn.Turn_next(this);
                yield break;
            }
        }
        else{
            // 공격할 my_color가 없으면 턴 종료
            Turn.Turn_next(this);
            yield break;
        }
    }

    public void Update_skill(){
        color = render.color;
        skills.Add(color);
    }
}