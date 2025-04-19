using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type17{
    public class skill313 : monoskill{
        public skill313() : base(313, "챠밍보이스", 40, 100, 17, 0, false){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/CharmingVoice"),
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