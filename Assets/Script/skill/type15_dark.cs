using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type15{
    public class skill223 : monoskill{
        public skill223() : base(223, "물기", 40, 100, 15, 0, true, 2){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;
            
            GameObject prefab = Resources.Load<GameObject>("Prefab/DarkBite");
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
    }
}