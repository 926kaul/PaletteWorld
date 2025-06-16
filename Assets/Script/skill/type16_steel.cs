using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type16
{
    public class skill122 : monoskill
    {
        public skill122() : base(122, "메탈클로", 40, 100, 16, 0, true, 2)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MetalClaw");
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
    public class skill022 : monoskill
    {
        public skill022() : base(022, "아이언헤드", 80, 100, 16, 16, true, 3, "상대에게 돌진하고, 충돌 시 최대 3만큼 밀어내며 피해를 준다")
        {
        }
        public IEnumerator skill_effect_with_result(y_color attacker, y_color defender, Action<bool> callback)
        {
            Rigidbody2D attackerRb = attacker.GetComponent<Rigidbody2D>();
            Rigidbody2D defenderRb = defender.GetComponent<Rigidbody2D>();

            if (attackerRb == null || defenderRb == null)
            {
                Debug.LogError("필수 Rigidbody2D 누락");
                callback(false);
                yield break;
            }

            Vector2 direction = (defender.transform.position - attacker.transform.position).normalized;
            float speed = 15f;

            float originalAttackerDrag = attackerRb.drag;
            float originalDefenderDrag = defenderRb.drag;

            attackerRb.drag = 0f;
            defenderRb.drag = 0.02f;

            attackerRb.velocity = direction * speed;
            attackerRb.gravityScale = 0;
            attackerRb.isKinematic = false;

            bool hitDefender = false;
            Vector3 defenderStart = defender.transform.position;
            float maxPushDistance = 3f;
            float timeout = 1.0f;
            float elapsed = 0f;

            CollisionListener2D listener = attacker.GetComponent<CollisionListener2D>();
            if (listener == null)
                listener = attacker.gameObject.AddComponent<CollisionListener2D>();

            listener.onCollisionEnter = (Collision2D col) =>
            {
                if (col.rigidbody == defenderRb)
                {
                    attackerRb.drag = 1000.0f;
                    hitDefender = true;
                }
            };

            while (elapsed < timeout)
            {
                if (hitDefender)
                {
                    float pushed = Vector3.Distance(defenderStart, defender.transform.position);
                    if (pushed >= maxPushDistance)
                    {
                        Vector3 limitedPos = defenderStart + (defender.transform.position - defenderStart).normalized * maxPushDistance;
                        defender.transform.position = limitedPos;
                        defenderRb.velocity = Vector2.zero;
                        break;
                    }
                }

                if (attackerRb.velocity.magnitude < 0.01f)
                    break;

                elapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            attackerRb.velocity = Vector2.zero;
            attackerRb.drag = originalAttackerDrag;
            defenderRb.drag = originalDefenderDrag;

            yield return new WaitForSeconds(0.2f);
            callback(hitDefender);
        }

        public override IEnumerator use_skill(y_color attacker, y_color defender)
        {

            int hit_score = (100 - this.accuracy) / 5 + Math.Max((this.phy ? defender.B : defender.D) - (this.phy ? attacker.A : attacker.C), 0) / 2;
            int hit_dice = rnd.Next(1, 21);
            int dicy_point = 0;

            if ((this.type1 == attacker.type1) || (this.type1 == attacker.type2)) //자속보정으로 유리보정
                dicy_point += 1;

            //원거리 불리보정 x

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

            if (attacker.cc.effect(hit_dice)) //effect 성공여부에 따라 데미지 여부 조정
            {
                bool hitSuccess = false;
                yield return skill_effect_with_result(attacker, defender, result => hitSuccess = result);
                if (!hitSuccess) hit_score = 30; ;
                (bool hit, int damage_score) = this.calc_skill(attacker, defender, hit_dice, hit_score);
                yield return defender.damaged(hit, damage_score);
                ApplyAdditional(hit, attacker, defender, damage_score);
            }
            else
            {
                yield break;
            }
        }
    }
}