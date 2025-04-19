using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type_{
    public class skill311 : monoskill{
        public skill311() : base(311, "불꽃세례", 40, 100, 1, 0, false){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject triangle = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/RedTriangle"),
                attacker.transform.position,
                Quaternion.identity
            );

            shooting_effect proj = triangle.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
            }
    }
}