using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type14{
    public class skill112 : monoskill{
        public skill112() : base(112, "용의숨결", 40, 100, 14, 0, false){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/DragonBreath"),
                attacker.transform.position,
                Quaternion.identity
            );

            shooting_effect proj = go.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
            }
    }
}