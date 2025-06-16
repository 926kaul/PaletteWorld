using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type7
{
    public class skill231 : monoskill
    {
        public skill231() : base(231, "스모그", 40, 100, 7, 0, false, 100)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/PoisonSmog");
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
    public class skill240 : monoskill
    {
        public skill240() : base(240, "맹독", 0, 100, 7, 7, false, 100, "상대를 맹독 상태로 만든다.")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/PoisonSkull");
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
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if((defender.cc is ncc)&&hit){
                defender.cc = new ppsn(defender);
            }
            return;
        }
    }
    
}