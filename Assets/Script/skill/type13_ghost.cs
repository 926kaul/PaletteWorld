using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type13{
    public class skill212 : monoskill{
        public skill212() : base(212, "햝기", 40, 100, 13, 0, true, 2){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;
        
            GameObject prefab = Resources.Load<GameObject>("Prefab/GhostTongue");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            shooting_effect proj = go.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
            }
    }
}