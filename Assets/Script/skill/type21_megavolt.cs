using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type21{
    public class skill332 : monoskill{
        public skill332() : base(332, "볼부비부비", 20, 100, 21, 0, true, 2){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MegavoltNuzzle");
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
            if(defender.cc is ncc && hit){
                defender.cc = new par(defender);
            }
            return;
        }
    }
}