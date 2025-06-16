using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type13
{
    public class skill212 : monoskill
    {
        public skill212() : base(212, "햝기", 40, 100, 13, 0, true, 2)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
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
    public class skill202 : monoskill
    {
        public skill202() : base(202, "섀도볼", 80, 100, 13, 13, false, 8, "20% 확률로 상대의 특수방어를 5 떨어뜨린다")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/FireBlast");
            prefab.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0f, 0.5f, 1f);
            prefab.transform.localScale = new Vector3(-0.35f, -0.35f, 1f);
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            shooting_with_particles proj = go.GetComponent<shooting_with_particles>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score){
            System.Random rnd = new System.Random();
            int dice = rnd.Next(1,21);
            if((dice >= 17)&&hit){
                defender.D = Math.Max(defender.D - 5, 0);
            }
            return;
        }
    }
}