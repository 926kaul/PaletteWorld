using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type22{
    public class skill233 : monoskill{
        public skill233() : base(233, "얼어붙은\n바람", 80, 100, 22, 0, false, 100, "30% 확률로 상대를 동상 상태로 만든다"){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MidwinterIcewind");
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
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score){
            System.Random rnd = new System.Random();
            int dice = rnd.Next(1,21);
            if((dice >= 15)&&(defender.cc is ncc) && hit){
                defender.cc = new frz(defender);
            }
            return;
        }
    }
}