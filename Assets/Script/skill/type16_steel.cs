using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type16{
    public class skill122 : monoskill{
        public skill122() : base(122, "메탈클로", 40, 100, 16, 0, true, 2){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject go = UnityEngine.Object.Instantiate(
                Resources.Load<GameObject>("Prefab/MetalClaw"),
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