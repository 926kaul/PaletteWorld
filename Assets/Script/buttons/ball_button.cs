using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor;
public class ball_button : MonoBehaviour
{
    public TextMeshPro mainText;
    public my_color selected;
    public start_button startButton;
    public List<Sprite> ballballs;
    public SpriteRenderer ballball;
    public int ball_mode = 0;
    
    void Update()
    {
        if (!start_button.started){
            mainText.text = "";
            ball_mode = 0;
            ballball.sprite = ballballs[ball_mode];
            ballball.color = new Color(0,0,0,0);
        }
        else if (start_button.started && GlobalVariables.selected_color != null){
            mainText.text = "Ball";
            ballball.color = new Color(255,255,255,255);
            ballball.sprite = ballballs[ball_mode];
        }
        else{
            mainText.text = "";
            ballball.color = new Color(0,0,0,0);
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)){
            ball_mode = (ball_mode + 1) % ballballs.Count;
            ballball.sprite = ballballs[ball_mode];
        }
    }
    void OnMouseDown(){
        selected = GlobalVariables.selected_color;
        if(Turn.turn_order.Count > 0 && selected == Turn.turn_order[0]){
            GlobalVariables.selected_skill = new ball();
        }
    }
}


public class ball: InTurn{
    System.Random rnd = new System.Random();
    public diceRollUI diceUI;
    public ball_button ballbutton = GameObject.FindObjectOfType<ball_button>();
    public virtual IEnumerator use_ball(y_color attacker, y_color defender){
        int hit_score = 20*defender.hp/(55+3*defender.H);
        int hit_dice = rnd.Next(1,21);
        if (diceUI == null)
            diceUI = GameObject.FindObjectOfType<diceRollUI>();

        yield return this.skill_effect(attacker, defender);
        yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));
        yield return result_of_ball(attacker,defender,hit_dice, hit_score);
    }

    public virtual IEnumerator skill_effect(y_color attacker, y_color defender){
        bool arrived = false;
        string prefab_name="Prefab/empty_ball";
        switch (ballbutton.ball_mode){
            case 0:
                prefab_name = "Prefab/empty_ball";
                break;
            case 1:
                prefab_name = "Prefab/lux_ball";
                break;
            case 2:
                prefab_name = "Prefab/color_ball";
                break;
            case 3:
                prefab_name = "Prefab/palette_ball";
                break;
        }
        GameObject aball = UnityEngine.Object.Instantiate(
            Resources.Load<GameObject>(prefab_name),
            attacker.transform.position,
            Quaternion.identity
        );

        shooting_effect proj = aball.GetComponent<shooting_effect>();
        proj.target = defender.transform.position;
        proj.onArrive = () => { arrived = true; };
        yield return new WaitUntil(() => arrived);
    }
    public IEnumerator result_of_ball(y_color attacker, y_color defender, int hit_dice, int hit_score){
        if(hit_dice < hit_score){
            yield return defender.damaged(false, 0);
            yield break;
        }
        yield return defender.damaged(true, 100);
        switch(ballbutton.ball_mode){
            case 0:
                UnityEngine.Object.Destroy(defender.gameObject);
                break;
            case 1:
                // 가산혼합: 각 성분별 최대값 선택
                attacker.color = new Color(
                    Mathf.Max(attacker.color.r, defender.color.r),
                    Mathf.Max(attacker.color.g, defender.color.g),
                    Mathf.Max(attacker.color.b, defender.color.b),
                    255
                );
                attacker.render.color = attacker.color;
                ((my_color)attacker).level += 1;
                UnityEngine.Object.Destroy(defender.gameObject);
                break;

            case 2:
                // 감산혼합: 각 성분별 최소값 선택
                attacker.color = new Color(
                    Mathf.Min(attacker.color.r, defender.color.r),
                    Mathf.Min(attacker.color.g, defender.color.g),
                    Mathf.Min(attacker.color.b, defender.color.b),
                    255
                );
                attacker.render.color = attacker.color;
                ((my_color)attacker).level += 1;
                UnityEngine.Object.Destroy(defender.gameObject);
                break;

            case 3:
                // 중간혼합: (attacker + defender) / 2
                attacker.color = new Color(
                    (attacker.color.r+ defender.color.r)/2,
                    (attacker.color.g+ defender.color.g)/2,
                    (attacker.color.b+ defender.color.b)/2,
                    255
                );
                attacker.render.color = attacker.color;
                ((my_color)attacker).level += 1;
                UnityEngine.Object.Destroy(defender.gameObject);
                break;
        }
        Debug.Log("attacker color: " + attacker.color);
        Debug.Log("attacker level: " + ((my_color)attacker).level);
        attacker.Update_stat();
        ((my_color)attacker).Update_skill();
    }
}
