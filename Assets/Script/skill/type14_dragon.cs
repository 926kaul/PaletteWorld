using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type14{
    public class skill112 : monoskill{
        public skill112() : base(112, "용의 분노", 10, 100, 14, 0, false){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;
            
            GameObject prefab = Resources.Load<GameObject>("Prefab/DragonBreath");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );


            shooting_effect proj = go.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
        }

        public override (bool,int) calc_skill(y_color attacker, y_color defender,int hit_dice, int hit_score){
            if(hit_dice==20||(hit_dice!=1&&(hit_score<=hit_dice))){
                Debug.Log("HIT");
                float typevs = every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2];

                int damage_score = 10 * ((typevs==0)?0:1);

                damage_score = Mathf.Max(damage_score,0);
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if(defender.hp <= 0){
                    UnityEngine.Object.Destroy(defender.gameObject);
                }
                return (true, damage_score);
            }
            else{
                Debug.Log($"{this.name} MISS");
                return (false, 0);
            }
        }
    }
}