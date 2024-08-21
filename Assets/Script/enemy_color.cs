using System.Collections;
using System.Collections.Generic;
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
    }
    void Update(){
        if(Turn.turn_order.Count > 0 && this == Turn.turn_order[0]){
            System.Random rnd = new System.Random();
            int skill_index = rnd.Next(skills.Count);

            Collider2D[] hitColliders = Physics2D.OverlapAreaAll(new Vector2(0,0), new Vector2(18,18), LayerMask.GetMask("Default"));
            List<my_color> my_colors = new List<my_color>();
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.GetComponent<my_color>() != null)
                {
                    my_colors.Add(collider.GetComponent<my_color>());
                }
            }
            int target_index = rnd.Next(my_colors.Count);

            use_skill(my_colors[target_index],every_skill.get_skill(skills[skill_index]));
        }
    }

    public void use_skill(y_color enemy, every_skill.monoskill used_skill){
        System.Random rnd = new System.Random();

        int hit_score = (100-used_skill.accuracy)/5 + Mathf.Max(enemy.H-(used_skill.adp?this.A:this.C),0) - ((this.type==used_skill.type)?5:0);
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
            int damage_score = used_skill.damage * Mathf.Max(damage_dice + (used_skill.adp?this.A:this.C) - (used_skill.adp?enemy.B:enemy.D),0) / 100;
            if(every_skill.typevs[used_skill.type, enemy.type]==0){
                damage_score = 0;
            }
            if(damage_dice==20){
                damage_score = used_skill.damage * (damage_dice + (used_skill.adp?this.A:this.C)) * 2 /100;
            }
            Debug.Log($"enemy gives you {damage_score} damage");
            enemy.hp -= damage_score;
            if(enemy.hp <= 0){
                Destroy(enemy.gameObject);
            }
        }
        else{
            Debug.Log("enemy MISS");
        }
        Turn.Turn_next(this);
    }

    void Update_skill(){
        color = render.color;
        float[] skill_color = {Mathf.Round(color.r * 255/64)/4, Mathf.Round(color.g * 255/64)/4, Mathf.Round(color.b * 255/64)/4};
        skills.Add(new Color(skill_color[0],skill_color[1],skill_color[2],255));
    }
}