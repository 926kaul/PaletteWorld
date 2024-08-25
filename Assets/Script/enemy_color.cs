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


    void Update_skill(){
        color = render.color;
        skills.Add(color);
    }
}