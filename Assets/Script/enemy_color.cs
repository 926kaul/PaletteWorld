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

            if (cc.effect()) {
                yield return StartCoroutine(UseSkillRoutine(my_colors[target_index], every_skill.get_skill(skills[skill_index])));
            }

            Turn.Turn_next(this);
        }

    }



    void Update_skill(){
        color = render.color;
        skills.Add(color);
    }
}