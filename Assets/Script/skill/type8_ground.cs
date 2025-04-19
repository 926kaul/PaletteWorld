using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type8{
    public class skill132 : monoskill{
        public skill132() : base(132, "진흙뿌리기", 40, 100, 8, 0, true){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/BrownMud"),
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