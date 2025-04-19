using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type7{
    public class skill231 : monoskill{
        public skill231() : base(231, "스모그", 40, 100, 7, 0, false, 100){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/PoisonSmog"),
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