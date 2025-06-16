using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type14
{
    public class skill112 : monoskill
    {
        public skill112() : base(112, "용의 분노", 10, 100, 14, 0, false)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/DragonBreath");
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

        public override (bool, int) calc_skill(y_color attacker, y_color defender, int hit_dice, int hit_score)
        {
            if (hit_dice == 20 || (hit_dice != 1 && (hit_score <= hit_dice)))
            {
                Debug.Log("HIT");
                float typevs = every_skill.typevs[this.type1, defender.type1] * every_skill.typevs[this.type1, defender.type2];

                int damage_score = 10 * ((typevs == 0) ? 0 : 1);

                damage_score = Mathf.Max(damage_score, 0);
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if (defender.hp <= 0)
                {
                    defender.Kill();
                }
                return (true, damage_score);
            }
            else
            {
                Debug.Log($"{this.name} MISS");
                return (false, 0);
            }
        }
    }
    public class skill002 : monoskill
    {
        public skill002() : base(002, "용성군", 130, 90, 14, 14, false, 100, "광역 상대(3), 사용 후 자신의 특수공격이 절반이 된다")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            yield return Explosion.Exploding(
                prefabPath: "Prefab/FireExplosion",
                position: defender.transform.position,
                growDuration: 0.45f,
                fadeDuration: 0.15f,
                maxScale: 18f,
                startAlpha: 0.1f,
                endAlpha: 0.8f,
                startColor: new Color(0f, 0f, 64f / 255f),
                endColor: new Color(0f, 0f, 191 / 255f)
            );
        }

        public override IEnumerator use_skill(y_color attacker, y_color defender)
        {

            int hit_score = (100 - this.accuracy) / 5 + Math.Max((this.phy ? defender.B : defender.D) - (this.phy ? attacker.A : attacker.C), 0) / 2;
            int hit_dice = rnd.Next(1, 21);
            int dicy_point = 0;

            if ((this.type1 == attacker.type1) || (this.type1 == attacker.type2)) //자속보정으로 유리보정
                dicy_point += 1;

            if (this.efrange > 3)
            {
                Type targetType = (attacker is my_color) ? typeof(enemy_color) : typeof(my_color);

                y_color[] allUnits = GameObject.FindObjectsOfType<y_color>();
                foreach (y_color unit in allUnits)
                {
                    if (unit.GetType() != targetType) continue;

                    if (unit.cc is ncc && Vector3.Distance(attacker.transform.position, unit.transform.position) <= 3f)
                    {
                        dicy_point -= 1;
                        break;
                    }
                }
            } // 원거리 (사거지 3초과)인 기술을 쓰는데 상태이상이 없는 상대가 거리 3이하에 있으면 압박을 받아 불리보정


            if (dicy_point > 0)
            {
                int hit_dice2 = rnd.Next(1, 21);
                if (diceUI == null)
                    diceUI = GameObject.FindObjectOfType<diceRollUI>();
                yield return diceUI.StartCoroutine(diceUI.AdvantageRoll(hit_dice, hit_dice2, hit_score));
                hit_dice = Math.Max(hit_dice, hit_dice2);
            }
            else if (dicy_point == 0)
            {
                if (diceUI == null)
                    diceUI = GameObject.FindObjectOfType<diceRollUI>();
                yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));
            }
            else
            {
                int hit_dice2 = rnd.Next(1, 21);
                if (diceUI == null)
                    diceUI = GameObject.FindObjectOfType<diceRollUI>();
                yield return diceUI.StartCoroutine(diceUI.DisadvantageRoll(hit_dice, hit_dice2, hit_score));
                hit_dice = Math.Min(hit_dice, hit_dice2);
            }

            if (attacker.cc.effect(hit_dice))
            {
                yield return this.skill_effect(attacker, defender);
                bool hit; int damage_score;
                y_color[] allUnits = GameObject.FindObjectsOfType<y_color>();
                Type targetType = (attacker is my_color) ? typeof(enemy_color) : typeof(my_color);

                foreach (y_color unit in allUnits)
                {
                    if (Vector3.Distance(defender.transform.position, unit.transform.position) <= 3f)
                    {
                        if (unit.GetType() != targetType) continue;
                        hit_score = (100 - this.accuracy) / 5 + Math.Max((this.phy ? unit.B : unit.D) - (this.phy ? attacker.A : attacker.C), 0) / 2;
                        (hit, damage_score) = this.calc_skill(attacker, unit, hit_dice, hit_score);
                        CoroutineRunner.Instance.StartCoroutine(unit.damaged(hit, damage_score));
                        ApplyAdditional(hit, attacker, unit, damage_score);
                    }
                }

            }
            else
            {
                yield break;
            }
        }

        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if (hit)
                attacker.C /= 2;
        }
        
    }
}