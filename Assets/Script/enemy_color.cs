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
        hp = 55 + 3*H;
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
            if(enemy_selected_skill.skill_availablity(this,my_colors[target_index])){
                if (cc.effect()) {
                yield return StartCoroutine(UseSkillRoutine(my_colors[target_index], enemy_selected_skill));
                Turn.Turn_next(this);
                }
                else if (distance <= 1){
                    Turn.Turn_next(this);
                    yield break;
                }
                else {
                    y_color target = my_colors[target_index];
                    Vector3 startPos = transform.position;
                    Vector3 targetPos = target.transform.position;
                    // 방향 벡터 계산 (정규화된 단위벡터)
                    Vector3 dir = (targetPos - startPos).normalized;
                    // distance 만큼 이동 가능한 거리 계산
                    float maxMove = Mathf.Min(distance, Vector3.Distance(startPos, targetPos) - 1f); // 최소 1칸은 남겨둬
                    // 최종 이동 좌표
                    Vector3 newPos = startPos + dir * maxMove;
                    // 이동 위치는 정수화 및 충돌 체크 필요
                    Vector3 rounded = new Vector3(Mathf.Round(newPos.x), Mathf.Round(newPos.y), 0f);
                    // 충돌 체크 (반지름 0.4, Default 레이어)
                    Collider2D[] overlaps = Physics2D.OverlapCircleAll(rounded, 0.3f, LayerMask.GetMask("Default"));
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
                    {
                        transform.position = rounded;
                        distance -= (int)Mathf.Round(Vector3.Distance(startPos, rounded));
                    }
                    yield break;
                }
            }
        }
    }



    void Update_skill(){
        color = render.color;
        skills.Add(color);
    }
}