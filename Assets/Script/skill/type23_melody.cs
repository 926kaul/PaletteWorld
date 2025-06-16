using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type23
{
    public class skill323 : monoskill
    {
        public skill323() : base(323, "드레인키스", 50, 100, 23, 0, false, 2, "가한 피해의 절반만큼 체력을 회복한다.")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MelodyKiss");
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
            if (hit)
                attacker.hp = Math.Min(attacker.hp + damage_score / 2, attacker.full_hp());
        }

    }
    public class skill424 : monoskill
    {
        public skill424() : base(424, "문포스", 95, 100, 23, 23, false, 10, "30% 확률로 상대의 특수공격을 5 떨어뜨린다")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MelodyMoon");
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
            if (hit)
            {
                System.Random rnd = new System.Random();
                int dice = rnd.Next(1, 21);
                if (dice >= 15)
                    defender.C = Math.Max(defender.C - 5, 0);
            }
            return;
        }
    }
}