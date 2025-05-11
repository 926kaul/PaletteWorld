using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type23{
    public class skill323 : monoskill{
        public skill323() : base(323, "드레인키스", 50, 100, 23, 0, false, 2){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MelodyKiss");
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
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score){
            attacker.hp = Math.Min(attacker.hp + damage_score/2, attacker.full_hp());
        }
        
    }
}