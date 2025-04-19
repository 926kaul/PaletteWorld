using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type5{
    public class skill133 : monoskill{
        public skill133() : base(133, "얼음뭉치", 40, 100, 5, 0, true){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/CoolIce"),
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