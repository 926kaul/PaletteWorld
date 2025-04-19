using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type12{
    public class skill221 : monoskill{
        public skill221() : base(221, "돌떨구기", 40, 100, 12, 0, true, 100){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/RockRock"),
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