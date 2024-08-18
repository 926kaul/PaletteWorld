using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using System;

public class skill_monitor : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer render;
    public int monitor_number;
    public int skill_mode; //-1 is inactive, 0 is pending, 1 is using
    public my_color selected;
    public every_skill.monoskill used_skill;
    void Start()
    {
        string objectName = gameObject.name;
        monitor_number =  int.Parse(Regex.Match(objectName, @"\d+").Value);
        GlobalVariables.skill_monitors[monitor_number] = this;
        render = GetComponent<SpriteRenderer>();
        render.color = new Color32(0,0,0,255);
        render.sortingLayerName = "Background";
        skill_mode = -1;
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
        }
        if (Input.GetMouseButtonDown(0) && skill_mode==1 && selected == every_skill.turn_order[0]){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null){
                enemy_color enemy = hit.collider.GetComponent<enemy_color>();
                System.Random rnd = new System.Random();
                if(enemy != null){
                    int hit_score = (100-used_skill.accuracy)/5 + Mathf.Max(enemy.H-(used_skill.adp?selected.A:selected.C),0) - ((selected.type==used_skill.type)?5:0);
                    int hit_dice = rnd.Next(1,21);
                    if(hit_dice==20||hit_dice!=1||hit_score<=hit_dice){
                        Debug.Log("HIT");
                        int damage_dice = rnd.Next(1,21);
                        if(every_skill.typevs[used_skill.type, enemy.type]!=1){
                            int damage_dice2 = rnd.Next(1,21);
                            if(every_skill.typevs[used_skill.type, enemy.type]==2){
                                damage_dice = Mathf.Max(damage_dice,damage_dice2);
                            }
                            else{
                                damage_dice = Mathf.Min(damage_dice,damage_dice2);
                            }
                            Debug.Log($"{damage_dice},{damage_dice2}");
                        }
                        int damage_score = used_skill.damage * Mathf.Max(damage_dice + (used_skill.adp?selected.A:selected.C) - (used_skill.adp?enemy.B:enemy.D),0) / 100;
                        if(every_skill.typevs[used_skill.type, enemy.type]==0){
                            damage_score = 0;
                        }
                        if(damage_dice==20){
                            damage_score = used_skill.damage * (damage_dice + (used_skill.adp?selected.A:selected.C)) * 2 /100;
                        }
                        Debug.Log($"{damage_score} damage");
                        enemy.hp -= damage_score;
                        if(enemy.hp <= 0){
                            Destroy(enemy.gameObject);
                        }
                    }
                    else{
                        Debug.Log("MISS");
                    }
                    every_skill.Turn_next(selected);
                }
            }
        }
    }
    void OnMouseDown(){
        if(skill_mode==0){
            used_skill = every_skill.get_skill(render.color);
            skill_mode=1;
        }
    }

}
