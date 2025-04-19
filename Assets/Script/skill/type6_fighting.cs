using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type6{
    public class skill211 : monoskill{
        public skill211() : base(211, "마하펀치", 40, 100, 6, 0, true, 3){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/MahaPunch"),
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