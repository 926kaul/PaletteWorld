using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type3{
    public class skill131 : monoskill{
        public skill131() : base(131, "나뭇잎", 40, 100, 3, 0, true){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject diamond = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/GreenDiamond"),
                attacker.transform.position,
                Quaternion.identity
            );

            shooting_effect proj = diamond.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
        }
    }
}