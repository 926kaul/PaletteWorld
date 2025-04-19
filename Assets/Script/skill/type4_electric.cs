using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type4{
    public class skill331 : monoskill{
        public skill331() : base(331, "전기쇼크", 40, 100, 4, 0, false){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/YellowThunder"),
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