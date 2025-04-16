using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type2{
    public class skill113 : monoskill{
        public skill113() : base(113, "물대포", 40, 100, 2, 0, false){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject circle = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/BlueCircle"),
                attacker.transform.position,
                Quaternion.identity
            );

            shooting_effect proj = circle.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
        }
    }
}