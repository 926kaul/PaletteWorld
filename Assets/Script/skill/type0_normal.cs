using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type0
{
    public class skill111 : monoskill
    {
        public skill111() : base(111, "자폭", 200, 100, 0, 25, true, 2, "광역 상대(2), 자신이 폭발해 적에게 피해를 입히고 죽게 된다")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            yield return Explosion.Exploding(
                prefabPath: "Prefab/FireExplosion",
                position: attacker.transform.position,
                growDuration: 0.45f,
                fadeDuration: 0.15f,
                maxScale: 12f,
                startAlpha: 0.1f,
                endAlpha: 0.8f,
                startColor: new Color(0f, 0f, 0f),
                endColor: new Color(0.5f, 0.5f, 0.5f)
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
                    if (Vector3.Distance(attacker.transform.position, unit.transform.position) <= 2f)
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
            attacker.hp = 0;
            UnityEngine.Object.Destroy(attacker.gameObject);
        }

    }
    public class skill221 : monoskill
    {
        public skill221() : base(221, "대폭발", 250, 90, 0, 12, true, 3, "광역 전체(3), 자신이 폭발해 모두에게 피해를 입히고 죽게 된다")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            yield return Explosion.Exploding(
                prefabPath: "Prefab/FireExplosion",
                position: attacker.transform.position,
                growDuration: 0.45f,
                fadeDuration: 0.15f,
                maxScale: 18f,
                startAlpha: 0.1f,
                endAlpha: 0.8f,
                startColor: new Color(0f, 0f, 0f),
                endColor: new Color(0.5f, 0.5f, 0.25f)
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
                foreach (y_color unit in allUnits)
                {
                    if (Vector3.Distance(attacker.transform.position, unit.transform.position) <= 3f)
                    {
                        if (unit == attacker) continue;
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
            attacker.hp = 0;
            UnityEngine.Object.Destroy(attacker.gameObject);
        }
    }
    public class skill121 : monoskill
    {
        public skill121() : base(121, "뿔드릴", 0, 30, 0, 11, true, 1, "30% 확률로 상대를 처치한다")
        {
        }

        public override IEnumerator use_skill(y_color attacker, y_color defender)
        {

            int hit_score = 15;
            int hit_dice = rnd.Next(1, 21);

            if (diceUI == null)
                diceUI = GameObject.FindObjectOfType<diceRollUI>();
            yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));

            if (attacker.cc.effect(hit_dice))
            {
                yield return this.skill_effect(attacker, defender);
                (bool hit, int damage_score) = this.calc_skill(attacker, defender, hit_dice, hit_score);
                yield return defender.damaged(hit, damage_score);
                ApplyAdditional(hit, attacker, defender, damage_score);
            }
            else
            {
                yield break;
            }
        }

        public override (bool, int) calc_skill(y_color attacker, y_color defender, int hit_dice, int hit_score)
        {
            if (hit_dice >= 15)
            {
                Debug.Log("HIT");
                float typevs = every_skill.typevs[this.type1, defender.type1] * every_skill.typevs[this.type1, defender.type2];
                int damage_score = (typevs == 1 ? 1 : 0) * defender.hp;

                Debug.Log($"damage : {this.damage}, typevs {this.type1} vs {defender.type1} : {typevs}");
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if (defender.hp <= 0)
                {
                    UnityEngine.Object.Destroy(defender.gameObject);
                }
                return (true, damage_score);
            }
            else
            {
                Debug.Log($"{this.name} MISS");
                return (false, 0);
            }
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool destroyed = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalDrill");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            shooting_effect_endless proj = go.GetComponent<shooting_effect_endless>();
            proj.target = defender.transform.position;
            proj.onDestroyCallback = () => { destroyed = true; };

            yield return new WaitUntil(() => destroyed);
        }
    }

    public class skill131 : monoskill
    {
        public skill131() : base(131, "HP 회복", 0, 100, 0, 3, false, 100, "자신의 전체 체력의 일부(최대 50%)를 회복한다 ")
        {
        }

        public override IEnumerator use_skill(y_color attacker, y_color defender)
        {

            int hit_score = 1;
            int hit_dice = rnd.Next(1, 21);

            if (diceUI == null)
                diceUI = GameObject.FindObjectOfType<diceRollUI>();
            yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));

            if (attacker.cc.effect(hit_dice))
            {
                (bool hit, int damage_score) = this.calc_skill(attacker, defender, hit_dice, hit_score);
                yield return attacker.damaged(hit, damage_score);
            }
            else
            {
                yield break;
            }
        }

        public override (bool, int) calc_skill(y_color attacker, y_color defender, int hit_dice, int hit_score)
        {
            if (hit_dice != 1)
            {
                Debug.Log("HIT");
                int damage_score = attacker.hp * hit_dice / 40;

                Debug.Log($"heal : {this.damage}");
                Debug.Log($"{this.name} heal {damage_score}");
                attacker.hp = Mathf.Min(attacker.full_hp(), attacker.hp + damage_score);
                return (true, damage_score);
            }
            else
            {
                Debug.Log($"{this.name} MISS");
                return (false, 0);
            }
        }

    }

    public class skill211 : monoskill
    {
        public skill211() : base(211, "이판사판\n태클", 120, 100, 0, 6, true, 5, "상대에게 돌진하며, 충돌 하는 모든 색깔말에 피해 주고 1/4만큼 반동 피해를 받는다")
        {
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

            if (attacker.cc.effect(hit_dice)) //effect의 충돌리스트에 따라 데미지 여부 조정
            {
                List<y_color> hitTargets = null;
                yield return skill_effect_with_result(attacker, defender, result => hitTargets = result);

                if (hitTargets == null || hitTargets.Count == 0)
                    hit_score = 30;
                foreach (var target in hitTargets)
                {
                    hit_score = (100 - this.accuracy) / 5 + Math.Max((this.phy ? target.B : target.D) - (this.phy ? attacker.A : attacker.C), 0) / 2;
                    (bool hit, int damage_score) = this.calc_skill(attacker, target, hit_dice, hit_score);
                    yield return target.damaged(hit, damage_score);
                    yield return attacker.damaged(hit, damage_score / 4);
                    ApplyAdditional(hit, attacker, target, damage_score);
                }
            }
            else
            {
                yield break;
            }
        }

        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if (!hit) return;
            attacker.hp -= damage_score / 4;
            if (attacker.hp <= 0)
            {
                UnityEngine.Object.Destroy(attacker.gameObject);
            }
        }

        public IEnumerator skill_effect_with_result(y_color attacker, y_color defender, Action<List<y_color>> callback)
        {
            Rigidbody2D attackerRb = attacker.GetComponent<Rigidbody2D>();
            if (attackerRb == null)
            {
                Debug.LogError("attacker에 Rigidbody2D가 없습니다.");
                callback(new List<y_color>());
                yield break;
            }

            Vector2 direction = (defender.transform.position - attacker.transform.position).normalized;
            float speed = 15f;

            float originalDrag = attackerRb.drag;
            attackerRb.drag = 0f;
            attackerRb.velocity = direction * speed;
            attackerRb.gravityScale = 0;
            attackerRb.isKinematic = false;

            List<y_color> hitTargets = new List<y_color>();

            CollisionListener2D listener = attacker.GetComponent<CollisionListener2D>();
            if (listener == null)
                listener = attacker.gameObject.AddComponent<CollisionListener2D>();

            listener.onCollisionEnter = (Collision2D col) =>
            {
                y_color target = col.gameObject.GetComponent<y_color>();
                if (target != null && !hitTargets.Contains(target) && target != attacker)
                {
                    hitTargets.Add(target);
                    attackerRb.drag = 2.0f;
                }
            };

            float waitTime = 1.5f;
            float elapsed = 0f;

            while (attackerRb.velocity.magnitude > 0.01f && elapsed < waitTime)
            {
                elapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            attackerRb.velocity = Vector2.zero;
            attackerRb.drag = originalDrag;

            yield return new WaitForSeconds(0.3f);
            callback(hitTargets);
        }




    }

    public class skill231 : monoskill
    {
        public skill231() : base(231, "아픔나누기", 0, 100, 0, 7, false, 100, "자신과 상대가 체력을 나누어가진다")
        {
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
                (bool hit, int damage_score) = this.calc_skill(attacker, defender, hit_dice, hit_score);
                yield return attacker.damaged(hit, damage_score);
                yield return defender.damaged(hit, damage_score);
                ApplyAdditional(hit, attacker, defender, damage_score);
            }
            else
            {
                yield break;
            }
        }

        public override (bool, int) calc_skill(y_color attacker, y_color defender, int hit_dice, int hit_score)
        {
            if (hit_dice == 20 || (hit_dice != 1 && (hit_score <= hit_dice)))
            {
                Debug.Log("HIT");
                int damage_score = (attacker.hp + defender.hp) / 2;

                damage_score = Mathf.Max(damage_score, 0);
                Debug.Log($"{this.name} damage {damage_score}");

                attacker.hp = Math.Min(attacker.full_hp(), damage_score);
                defender.hp = Math.Min(defender.full_hp(), damage_score);

                if (defender.hp <= 0)
                {
                    UnityEngine.Object.Destroy(defender.gameObject);
                }
                if (attacker.hp <= 0)
                {
                    UnityEngine.Object.Destroy(attacker.gameObject);
                }
                return (true, damage_score);
            }
            else
            {
                Debug.Log($"{this.name} MISS");
                return (false, 0);
            }
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            SpriteRenderer attackerRenderer = attacker.GetComponent<SpriteRenderer>();
            SpriteRenderer defenderRenderer = defender.GetComponent<SpriteRenderer>();

            if (attackerRenderer == null || defenderRenderer == null)
                yield break;

            // 원래 색상 저장
            Color attColor = attackerRenderer.color;
            Color defColor = defenderRenderer.color;

            // 1. G값 swap
            Color attSwapped = new Color(attColor.r, defColor.g, attColor.b, attColor.a);
            Color defSwapped = new Color(defColor.r, attColor.g, defColor.b, defColor.a);

            attackerRenderer.color = attSwapped;
            defenderRenderer.color = defSwapped;
            yield return new WaitForSeconds(0.2f);

            // 2. G값 평균
            float avgG = (attColor.g + defColor.g) / 2f;
            Color attAvg = new Color(attSwapped.r, avgG, attSwapped.b, attSwapped.a);
            Color defAvg = new Color(defSwapped.r, avgG, defSwapped.b, defSwapped.a);

            attackerRenderer.color = attAvg;
            defenderRenderer.color = defAvg;
            yield return new WaitForSeconds(0.2f);

            // 3. 원래 색 복원
            attackerRenderer.color = attColor;
            defenderRenderer.color = defColor;
        }



    }

    public class skill311 : monoskill
    {
        public skill311() : base(311, "객기", 70, 100, 0, 1, true, 2, "자신에게 상태이상이 있을때 상태이상을 해제하고, 기술의 위력이 2배가 된다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            // 빈 이펙트 프리팹 로드 (SpriteRenderer 포함된 껍데기)
            GameObject basePrefab = Resources.Load<GameObject>("Prefab/NormalBody");
            GameObject go = UnityEngine.Object.Instantiate(
                basePrefab,
                attacker.transform.position,
                basePrefab.transform.rotation
            );

            // SpriteRenderer 복제
            SpriteRenderer attackerRenderer = attacker.GetComponent<SpriteRenderer>();
            SpriteRenderer projRenderer = go.GetComponent<SpriteRenderer>();
            projRenderer.sprite = attackerRenderer.sprite;
            projRenderer.color = attacker.cc.cc_color;
            projRenderer.sortingLayerName = "Effect";
            projRenderer.sortingOrder = 5;

            go.transform.localScale = attacker.transform.localScale;

            // 타겟 설정
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

                CC tmp = attacker.cc;

                if (!(attacker.cc is ncc))
                    attacker.cc = new ncc(attacker);

                float damage_dice = (float)rnd.Next(1, 4);
                float typevs = every_skill.typevs[this.type1, defender.type1] * every_skill.typevs[this.type1, defender.type2];
                float critical = 1.0f;
                float acbd = (float)Mathf.Max((this.phy ? attacker.A : attacker.C) - (this.phy ? defender.B : defender.D), 0);

                if (hit_dice == 20)
                {
                    critical = 2.0f;
                    acbd = (float)(this.phy ? attacker.A : attacker.C);
                    damage_dice = 4.0f;
                }

                int damage_score = (int)(((float)this.damage) / 100.00f * typevs * (acbd + 16 + damage_dice) * critical);

                damage_score = Mathf.Max(damage_score, 0);
                if (!(tmp is ncc))
                    damage_score *= 2; //객기 사용시 위력 2배
                Debug.Log($"damage : {this.damage}, typevs {this.type1} vs {defender.type1} : {typevs}, acbd {acbd}, damage_dice {damage_dice}, critical {critical}");
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if (defender.hp <= 0)
                {
                    UnityEngine.Object.Destroy(defender.gameObject);
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

    public class skill321 : monoskill
    {
        public skill321() : base(321, "대지의 파동", 50, 100, 0, 8, false, 100, "사용할 때 필드 상태에 따라 기술 타입이 바뀌고, 위력이 2배가 된다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalWave");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            Color32 fieldColor = new Color32(128, 128, 128, 255); // 기본 필드 색상 (회색)
            if (GlobalVariables.field == 1)
                fieldColor = new Color32(0, 255, 0, 255);
            else if (GlobalVariables.field == 2)
                fieldColor = new Color32(255, 255, 0, 255);
            else if (GlobalVariables.field == 3)
                fieldColor = new Color32(255, 0, 255, 255);
            else if (GlobalVariables.field == 4)
                fieldColor = new Color32(255, 128, 255, 255);

            go.GetComponent<SpriteRenderer>().color = fieldColor;
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

                if (GlobalVariables.field == 0)
                    this.type1 = 0;
                else if (GlobalVariables.field == 1)
                    this.type1 = 4; // 풀 타입
                else if (GlobalVariables.field == 2)
                    this.type1 = 5; // 전기 타입
                else if (GlobalVariables.field == 3)
                    this.type1 = 10; // 에스퍼 타입
                else if (GlobalVariables.field == 4)
                    this.type1 = 17; // 페어리 타입

                float damage_dice = (float)rnd.Next(1, 4);
                float typevs = every_skill.typevs[this.type1, defender.type1] * every_skill.typevs[this.type1, defender.type2];
                float critical = 1.0f;
                float acbd = (float)Mathf.Max((this.phy ? attacker.A : attacker.C) - (this.phy ? defender.B : defender.D), 0);

                if (hit_dice == 20)
                {
                    critical = 2.0f;
                    acbd = (float)(this.phy ? attacker.A : attacker.C);
                    damage_dice = 4.0f;
                }

                int damage_score = (int)(((float)this.damage) / 100.00f * typevs * (acbd + 16 + damage_dice) * critical);
                damage_score = WetherAndField(damage_score);

                damage_score = Mathf.Max(damage_score, 0);
                if (GlobalVariables.field != 0)
                    damage_score *= 2;
                Debug.Log($"damage : {this.damage}, typevs {this.type1} vs {defender.type1} : {typevs}, acbd {acbd}, damage_dice {damage_dice}, critical {critical}");
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if (defender.hp <= 0)
                {
                    UnityEngine.Object.Destroy(defender.gameObject);
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

    public class skill331 : monoskill
    {
        public skill331() : base(331, "뱀눈초리", 0, 100, 0, 4, true, 3)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalSnake");
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
            if ((defender.cc is ncc) && hit)
            {
                defender.cc = new par(defender);
            }
            return;
        }
    }

    public class skill222 : monoskill
    {
        public skill222() : base(222, "몸통박치기", 40, 100, 0, 26, true, 1)
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            // 빈 이펙트 프리팹 로드 (SpriteRenderer 포함된 껍데기)
            GameObject basePrefab = Resources.Load<GameObject>("Prefab/NormalBody");
            GameObject go = UnityEngine.Object.Instantiate(
                basePrefab,
                attacker.transform.position,
                basePrefab.transform.rotation
            );

            // SpriteRenderer 복제
            SpriteRenderer attackerRenderer = attacker.GetComponent<SpriteRenderer>();
            SpriteRenderer projRenderer = go.GetComponent<SpriteRenderer>();
            projRenderer.sprite = attackerRenderer.sprite;
            projRenderer.color = attackerRenderer.color;
            projRenderer.sortingLayerName = "Effect";
            projRenderer.sortingOrder = 5;

            go.transform.localScale = attacker.transform.localScale;

            // 타겟 설정
            shooting_effect proj = go.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };

            yield return new WaitUntil(() => arrived);
        }
    }

    public class skill112 : monoskill
    {
        public skill112() : base(112, "기가임팩트", 150, 90, 0, 14, true, 2, "사용 후 1턴 동안 움직이지 못한다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            // 빈 이펙트 프리팹 로드 (SpriteRenderer 포함된 껍데기)
            GameObject basePrefab = Resources.Load<GameObject>("Prefab/NormalBody");
            GameObject go = UnityEngine.Object.Instantiate(
                basePrefab,
                attacker.transform.position,
                basePrefab.transform.rotation
            );

            // SpriteRenderer 복제
            SpriteRenderer attackerRenderer = attacker.GetComponent<SpriteRenderer>();
            SpriteRenderer projRenderer = go.GetComponent<SpriteRenderer>();
            projRenderer.sprite = attackerRenderer.sprite;
            projRenderer.color = attackerRenderer.color;
            projRenderer.sortingLayerName = "Effect";
            projRenderer.sortingOrder = 5;

            go.transform.localScale = new Vector3(4f, 4f, 4f);


            // 타겟 설정
            shooting_effect proj = go.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };

            yield return new WaitUntil(() => arrived);
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            attacker.cc = new rbd(attacker); // 반동 1턴
        }
    }

    public class skill122 : monoskill
    {
        public skill122() : base(122, "돌진", 90, 85, 0, 16, true, 5, "상대에게 돌진하며, 충돌 시 피해 주고 1/4만큼 반동 피해를 받는다")
        {
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
                yield return attacker.damaged(hit, damage_score / 4);
                ApplyAdditional(hit, attacker, defender, damage_score);
            }
            else
            {
                yield break;
            }
        }

        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if (!hit) return;
            attacker.hp -= damage_score / 4;
            if (attacker.hp <= 0)
            {
                UnityEngine.Object.Destroy(attacker.gameObject);
            }
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

            // drag 보존 후 제거
            float originalDrag = attackerRb.drag;
            attackerRb.drag = 0f;

            attackerRb.velocity = direction * speed;
            attackerRb.gravityScale = 0;
            attackerRb.isKinematic = false;

            bool hitDefender = false;

            CollisionListener2D listener = attacker.GetComponent<CollisionListener2D>();
            if (listener == null)
                listener = attacker.gameObject.AddComponent<CollisionListener2D>();

            listener.onCollisionEnter = (Collision2D col) =>
            {
                attackerRb.drag = 2.0f;
                if (col.rigidbody == defenderRb)
                    hitDefender = true;
            };

            while (!hitDefender && attackerRb.velocity.magnitude > 0.01f)
                yield return new WaitForFixedUpdate();

            attackerRb.velocity = Vector2.zero;
            attackerRb.drag = originalDrag;  // drag 복원

            yield return new WaitForSeconds(0.3f);
            callback(hitDefender);
        }
    }
    public class skill132 : monoskill
    {
        public skill132() : base(132, "노래하기", 0, 55, 0, 24, false, 100, "상대를 잠듦 상태로 만든다.")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalSong");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            shooting_effect proj = go.GetComponent<shooting_effect>();
            SpriteRenderer attackerRenderer = attacker.GetComponent<SpriteRenderer>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if (defender.cc is ncc && hit)
            {
                defender.cc = new slp(defender);
            }
            return;
        }
    }

    public class skill212 : monoskill
    {
        public skill212() : base(212, "변신", 0, 100, 0, 13, false, 100, "상대와 같은 색깔말로 변신한다")
        {
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if (hit)
            {
                attacker.color = new Color32(
                    (byte)(defender.color.r),
                    (byte)(defender.color.g),
                    (byte)(defender.color.b),
                    255
                );
                attacker.render.color = attacker.color;
                attacker.Update_stat();
                ((my_color)attacker).Update_skill();
            }
            return;
        }
    }

    public class skill332 : monoskill
    {
        public skill332() : base(332, "칼춤", 0, 100, 0, 21, true, 3, "자신과 상대 하나의 공격(최대 31)을 2배로 증가시킨다")
        {
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if (hit)
            {
                attacker.A = Math.Min(attacker.A * 2, 31);
                defender.A = Math.Min(defender.A * 2, 31);
            }
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            // 1. 중간 위치 계산
            Vector3 midPos = (attacker.transform.position + defender.transform.position) / 2f;

            // 2. 프리팹 로드 및 생성
            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalSwords");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                midPos,
                Quaternion.identity
            );

            // 3. SpriteRenderer 가져오기
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr == null)
                sr = go.GetComponentInChildren<SpriteRenderer>();

            if (sr != null)
            {
                Color color = sr.color;
                color.a = 0f;
                sr.color = color;

                float duration = 0.3f;
                float elapsed = 0f;

                // 4. alpha 0 → 1로 부드럽게 증가
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    float alpha = Mathf.Clamp01(elapsed / duration);
                    color.a = alpha;
                    sr.color = color;
                    yield return null;
                }
            }

            // 5. 짧게 유지 후 제거
            yield return new WaitForSeconds(0.2f);
            UnityEngine.Object.Destroy(go);
        }
    }


    public class skill312 : monoskill
    {
        public skill312() : base(312, "파괴광선", 150, 90, 0, 17, false, 10, "사용 후 1턴 동안 움직이지 못한다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 start = attacker.transform.position;
            Vector3 end = defender.transform.position;
            Vector3 direction = (end - start);
            float fullLength = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 프리팹 생성: 위치는 attacker
            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalBeam");
            GameObject beam = UnityEngine.Object.Instantiate(prefab, start, Quaternion.Euler(0, 0, angle));

            // 처음엔 길이 0
            beam.transform.localScale = new Vector3(0f, 0.3f, 1f);

            // SpriteRenderer 세팅
            SpriteRenderer sr = beam.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "Effect";
                sr.sortingOrder = 5;
            }

            float growTime = 0.2f;
            float holdTime = 0.2f;
            float elapsed = 0f;

            while (elapsed < growTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / growTime);
                float curLength = t * fullLength;

                // scale 키우기
                beam.transform.localScale = new Vector3(curLength, 0.3f, 1f);

                // 위치 보정: 중앙 피벗을 고려해 beam의 중심을 start → 중간으로 이동
                beam.transform.position = start + (direction.normalized * curLength / 2f);

                yield return null;
            }

            // 최종 위치/스케일 고정
            beam.transform.localScale = new Vector3(fullLength, 0.3f, 1f);
            beam.transform.position = start + direction.normalized * fullLength / 2f;

            yield return new WaitForSeconds(holdTime);
            UnityEngine.Object.Destroy(beam);
        }



        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            attacker.cc = new rbd(attacker); // 반동 1턴
        }
    }

    public class skill322 : monoskill
    {
        public skill322() : base(322, "배북", 0, 100, 0, 18, true, 3, "자신의 공격을 31로 만들고, 최대 체력의 절반을 잃는다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 originalScale = attacker.transform.localScale;
            Vector3 enlargedScale = originalScale * 3f;

            float growTime = 0.2f;
            float shrinkTime = 0.2f;
            float holdTime = 0.1f;

            // 1. 점점 커지기
            float elapsed = 0f;
            while (elapsed < growTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / growTime);
                attacker.transform.localScale = Vector3.Lerp(originalScale, enlargedScale, t);
                yield return null;
            }

            // 2. 잠깐 유지
            attacker.transform.localScale = enlargedScale;
            yield return new WaitForSeconds(holdTime);

            // 3. 원래대로 돌아오기
            elapsed = 0f;
            while (elapsed < shrinkTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / shrinkTime);
                attacker.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, t);
                yield return null;
            }

            // 4. 정확히 원래 크기로 정렬
            attacker.transform.localScale = originalScale;
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            attacker.A = 31;
            attacker.hp = Mathf.Max(attacker.hp - attacker.full_hp() / 2, 1);
        }
    }

    public class skill232 : monoskill
    {
        public skill232() : base(232, "성장", 0, 100, 0, 20, false, 100, "자신의 공격과 특수공격이 5 증가한다. 날씨가 쾌청이라면 10 증가한다.")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 originalScale = attacker.transform.localScale;
            Vector3 enlargedScale = originalScale * 2f;

            float growTime = 0.2f;
            float shrinkTime = 0.2f;
            float holdTime = 0.1f;

            // 1. 점점 커지기
            float elapsed = 0f;
            while (elapsed < growTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / growTime);
                attacker.transform.localScale = Vector3.Lerp(originalScale, enlargedScale, t);
                yield return null;
            }

            // 2. 잠깐 유지
            attacker.transform.localScale = enlargedScale;
            yield return new WaitForSeconds(holdTime);

            // 3. 원래대로 돌아오기
            elapsed = 0f;
            while (elapsed < shrinkTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / shrinkTime);
                attacker.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, t);
                yield return null;
            }

            // 4. 정확히 원래 크기로 정렬
            attacker.transform.localScale = originalScale;
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            attacker.A = Math.Min(attacker.A + (GlobalVariables.weather == 1 ? 10 : 5), 31);
            attacker.C = Math.Min(attacker.C + (GlobalVariables.weather == 1 ? 10 : 5), 31);
        }
    }

    public class skill113 : monoskill
    {
        public skill113() : base(113, "튀어오르기", 0, 100, 0, 2, true, 100, "아무 일도 일어나지 않는다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 originalScale = attacker.transform.localScale;
            Vector3 enlargedScale = originalScale * 2f;

            float growTime = 0.2f;
            float shrinkTime = 0.2f;
            float holdTime = 0.1f;

            // 1. 점점 커지기
            float elapsed = 0f;
            while (elapsed < growTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / growTime);
                attacker.transform.localScale = Vector3.Lerp(originalScale, enlargedScale, t);
                yield return null;
            }

            // 2. 잠깐 유지
            attacker.transform.localScale = enlargedScale;
            yield return new WaitForSeconds(holdTime);

            // 3. 원래대로 돌아오기
            elapsed = 0f;
            while (elapsed < shrinkTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / shrinkTime);
                attacker.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, t);
                yield return null;
            }

            // 4. 정확히 원래 크기로 정렬
            attacker.transform.localScale = originalScale;
        }

        public virtual (bool, int) calc_skill(y_color attacker, y_color defender, int hit_dice, int hit_score)
        {
            if (hit_dice == 20 || (hit_dice != 1 && (hit_score <= hit_dice)))
            {
                Debug.Log("HIT");
                float damage_dice = (float)rnd.Next(1, 4);
                float typevs = every_skill.typevs[this.type1, defender.type1] * every_skill.typevs[this.type1, defender.type2];
                float critical = 1.0f;
                float acbd = (float)Mathf.Max((this.phy ? attacker.A : attacker.C) - (this.phy ? defender.B : defender.D), 0);

                if (hit_dice == 20)
                {
                    attacker.A = 31;
                    attacker.B = 31;
                    attacker.C = 31;
                    attacker.D = 31;
                    attacker.S = 31;
                    attacker.H = 31;
                    attacker.hp = attacker.full_hp();
                }

                int damage_score = (int)(((float)this.damage) / 100.00f * typevs * (acbd + 16 + damage_dice) * critical);
                damage_score = WetherAndField(damage_score);

                damage_score = Mathf.Max(damage_score, 0);
                Debug.Log($"damage : {this.damage}, typevs {this.type1} vs {defender.type1} : {typevs}, acbd {acbd}, damage_dice {damage_dice}, critical {critical}");
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if (defender.hp <= 0)
                {
                    UnityEngine.Object.Destroy(defender.gameObject);
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

    public class skill123 : monoskill
    {
        public skill123() : base(123, "웨더볼", 50, 100, 0, 9, false, 100, "사용할 때 날씨에 따라 기술 타입이 바뀌고, 위력이 2배가 된다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/BlueCircle");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            Color32 fieldColor = new Color32(128, 128, 128, 255); // 기본 필드 색상 (회색)
            if (GlobalVariables.weather == 1)
                fieldColor = new Color32(255, 0, 0, 255);
            else if (GlobalVariables.field == 2)
                fieldColor = new Color32(0, 255, 0, 255);
            else if (GlobalVariables.field == 3)
                fieldColor = new Color32(128, 128, 0, 255);
            else if (GlobalVariables.field == 4)
                fieldColor = new Color32(0, 255, 255, 255);

            go.GetComponent<SpriteRenderer>().color = fieldColor;
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

                if (GlobalVariables.field == 0)
                    this.type1 = 0;
                else if (GlobalVariables.field == 1)
                    this.type1 = 1; // 불 타입
                else if (GlobalVariables.field == 2)
                    this.type1 = 2; // 물 타입
                else if (GlobalVariables.field == 3)
                    this.type1 = 12; // 바위 타입
                else if (GlobalVariables.field == 4)
                    this.type1 = 5; // 얼음 타입

                float damage_dice = (float)rnd.Next(1, 4);
                float typevs = every_skill.typevs[this.type1, defender.type1] * every_skill.typevs[this.type1, defender.type2];
                float critical = 1.0f;
                float acbd = (float)Mathf.Max((this.phy ? attacker.A : attacker.C) - (this.phy ? defender.B : defender.D), 0);

                if (hit_dice == 20)
                {
                    critical = 2.0f;
                    acbd = (float)(this.phy ? attacker.A : attacker.C);
                    damage_dice = 4.0f;
                }

                int damage_score = (int)(((float)this.damage) / 100.00f * typevs * (acbd + 16 + damage_dice) * critical);
                damage_score = WetherAndField(damage_score);

                damage_score = Mathf.Max(damage_score, 0);
                if (GlobalVariables.field != 0)
                    damage_score *= 2;
                Debug.Log($"damage : {this.damage}, typevs {this.type1} vs {defender.type1} : {typevs}, acbd {acbd}, damage_dice {damage_dice}, critical {critical}");
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if (defender.hp <= 0)
                {
                    UnityEngine.Object.Destroy(defender.gameObject);
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

    public class skill133 : monoskill
    {
        public skill133() : base(133, "트라이어택", 80, 100, 0, 5, false, 100, "20% 확률로 상대에게 화상, 마비, 동상 상태를 중 하나를 부여한다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/BlueCircle");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            shooting_effect proj = go.GetComponent<shooting_effect>();
            proj.target = defender.transform.position;

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            Color[] colors = new Color[]
            {
                Color.red,
                Color.yellow,
                new Color(0f, 1f, 1f) // 하늘색
            };
            int colorIndex = 0;
            float colorChangeInterval = 0.05f;
            float elapsedColorTime = 0f;

            bool arrived = false;
            proj.onArrive = () => { arrived = true; };

            while (!arrived)
            {
                elapsedColorTime += Time.deltaTime;
                if (elapsedColorTime >= colorChangeInterval)
                {
                    sr.color = colors[colorIndex % colors.Length];
                    colorIndex++;
                    elapsedColorTime = 0f;
                }
                yield return null;
            }
        }

        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            System.Random rnd = new System.Random();
            int dice = rnd.Next(1, 21);
            if ((dice >= 19) && (defender.cc is ncc) && hit)
            {
                defender.cc = new frz(defender);
            }
            else if ((dice >= 18) && (defender.cc is ncc) && hit)
            {
                defender.cc = new par(defender);
            }
            else if ((dice >= 17) && (defender.cc is ncc) && hit)
            {
                defender.cc = new brn(defender);
            }
            return;
        }
    }

    public class skill213 : monoskill
    {
        public skill213() : base(213, "속이기", 30, 100, 0, 15, true, 3, "자신과 상대를 반동 상태로 만든다")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 direction = (defender.transform.position - attacker.transform.position).normalized;
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0f);

            float offset = 1.5f;
            float speed = 25f;
            float stopDistanceFromDefender = 0.5f;  // 여기 기준으로!
            float holdDuration = 0.1f;
            float maxTime = 1.0f;

            Vector3 leftStart = defender.transform.position + perpendicular * offset;
            Vector3 rightStart = defender.transform.position - perpendicular * offset;

            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalClap");
            GameObject left = UnityEngine.Object.Instantiate(prefab, leftStart, Quaternion.identity);
            GameObject right = UnityEngine.Object.Instantiate(prefab, rightStart, Quaternion.identity);

            Vector3 scale = left.transform.localScale;
            right.transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);

            Vector3 leftPos = leftStart;
            Vector3 rightPos = rightStart;

            Vector3 leftDir = (defender.transform.position - leftStart).normalized;
            Vector3 rightDir = (defender.transform.position - rightStart).normalized;

            float elapsed = 0f;
            while ((Vector3.Distance(leftPos, defender.transform.position) > stopDistanceFromDefender ||
                    Vector3.Distance(rightPos, defender.transform.position) > stopDistanceFromDefender) &&
                elapsed < maxTime)
            {
                float step = speed * Time.deltaTime;
                leftPos += leftDir * step;
                rightPos += rightDir * step;

                left.transform.position = leftPos;
                right.transform.position = rightPos;

                elapsed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(holdDuration);
            UnityEngine.Object.Destroy(left);
            UnityEngine.Object.Destroy(right);
        }


        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if (hit && (defender.cc is ncc))
            {
                attacker.cc = new rbd(attacker);
                defender.cc = new rbd(defender);
            }
            return;
        }
    }

    public class skill223 : monoskill
    {
        public skill223() : base(223, "스위프\n뺨치기", 25, 85, 0, 19, true, 3, "명중 시 이 기술을 다시 시전한다 (최대 5회)")
        {
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 attackDir = (defender.transform.position - attacker.transform.position).normalized;
            Vector3 perpendicular = new Vector3(attackDir.y, -attackDir.x, 0f); // 오른쪽 → 왼쪽

            float offset = 1.5f;
            float speed = 50f;
            float holdDuration = 0.05f;
            float maxTime = 1.0f;

            Vector3 startPos = defender.transform.position + perpendicular * offset;
            Vector3 endPos = defender.transform.position - perpendicular * offset;

            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalSlap");
            GameObject hand = UnityEngine.Object.Instantiate(prefab, startPos, Quaternion.identity);

            Vector3 dir = (endPos - startPos).normalized;

            // ✅ 이동 방향을 바라보도록 회전
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            hand.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Vector3 current = startPos;
            float elapsed = 0f;

            while (Vector3.Distance(current, endPos) > 0.05f && elapsed < maxTime)
            {
                float step = speed * Time.deltaTime;
                current += dir * step;
                hand.transform.position = current;

                elapsed += Time.deltaTime;
                yield return null;
            }

            hand.transform.position = endPos;

            yield return new WaitForSeconds(holdDuration);
            UnityEngine.Object.Destroy(hand);
        }


        public override IEnumerator use_skill(y_color attacker, y_color defender)
        {

            int repeatCount = 0;
            const int maxRepeat = 5;
            bool continueFire = true;

            while (continueFire && repeatCount < maxRepeat)
            {
                repeatCount++;

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

                bool hit = false;
                int damage_score = 0;
                if (attacker.cc.effect(hit_dice))
                {
                    yield return this.skill_effect(attacker, defender);
                    (hit, damage_score) = this.calc_skill(attacker, defender, hit_dice, hit_score);
                    yield return defender.damaged(hit, damage_score);
                    ApplyAdditional(hit, attacker, defender, damage_score);
                }
                else
                {
                    yield break;
                }

                // 적중하지 않으면 반복 종료
                continueFire = hit;
            }
        }
    }

    public class skill233 : monoskill
    {
        public skill233() : base(233, "죽기살기", 0, 100, 0, 22, false, 100, "상대의 체력을 자신과 동일하게 만든다")
        {
        }

        public override (bool, int) calc_skill(y_color attacker, y_color defender, int hit_dice, int hit_score)
        {
            if (hit_dice == 20 || (hit_dice != 1 && (hit_score <= hit_dice)))
            {
                Debug.Log("HIT");
                int damage_score = attacker.hp;

                damage_score = Mathf.Max(damage_score, 0);
                Debug.Log($"{this.name} damage {damage_score}");

                defender.hp = Math.Min(defender.full_hp(), damage_score);

                if (defender.hp <= 0)
                {
                    UnityEngine.Object.Destroy(defender.gameObject);
                }
                if (attacker.hp <= 0)
                {
                    UnityEngine.Object.Destroy(attacker.gameObject);
                }
                return (true, damage_score);
            }
            else
            {
                Debug.Log($"{this.name} MISS");
                return (false, 0);
            }
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            SpriteRenderer attackerRenderer = attacker.GetComponent<SpriteRenderer>();
            SpriteRenderer defenderRenderer = defender.GetComponent<SpriteRenderer>();

            if (attackerRenderer == null || defenderRenderer == null)
                yield break;

            // 원래 색상 저장
            Color attColor = attackerRenderer.color;
            Color defColor = defenderRenderer.color;

            // 1. G값 swap
            Color defSwapped = new Color(attColor.r, attColor.g, attColor.b, defColor.a);

            defenderRenderer.color = defSwapped;
            yield return new WaitForSeconds(0.2f);

            // 2. G값 평균
            float avgR = (attColor.r + defColor.r) / 2f;
            float avgG = (attColor.g + defColor.g) / 2f;
            float avgB = (attColor.b + defColor.b) / 2f;

            Color defAvg = new Color(avgR, avgG, avgB, defSwapped.a);

            defenderRenderer.color = defAvg;
            yield return new WaitForSeconds(0.2f);

            // 3. 원래 색 복원
            defenderRenderer.color = defColor;
        }
    }
    public class skill313 : monoskill
    {
        public skill313() : base(313, "손가락\n흔들기", 0, 100, 0, 10, false, 100, "무작위 스킬 하나를 사용한다")
        {
        }
        public override IEnumerator use_skill(y_color attacker, y_color defender)
        {
            System.Random rnd = new System.Random();
            monoskill skill = null;
            while (skill == null)
            {
                int skilltype = rnd.Next(0, 25);
                int skillr = rnd.Next(0, 5);
                int skillg = rnd.Next(0, 5);
                int skillb = rnd.Next(0, 5);
                skill = every_skill.skillset[skilltype, skillr, skillg, skillb];
            }
            yield return skill.use_skill(attacker, defender);
        }
    }

    public class skill323 : monoskill
    {
        public skill323() : base(323, "하이퍼\n보이스", 100, 100, 0, 24, false, 3, "광역 전체(3), 주변 모든 색깔에게 피해를 준다")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            yield return Explosion.Exploding(
                prefabPath: "Prefab/NormalVoice",
                position: attacker.transform.position,
                growDuration: 0.45f,
                fadeDuration: 0.15f,
                maxScale: 0.64f,
                startAlpha: 0.1f,
                endAlpha: 0.5f,
                startColor: new Color(0f, 0f, 0f),
                endColor: new Color(0.7f, 0.5f, 0.7f)
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

                y_color[] allUnits = GameObject.FindObjectsOfType<y_color>();
                foreach (y_color unit in allUnits)
                {
                    if (unit == attacker) continue;
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
                foreach (y_color unit in allUnits)
                {
                    if (Vector3.Distance(attacker.transform.position, unit.transform.position) <= 3f)
                    {
                        if (unit == attacker) continue;
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
    }

    public class skill333 : monoskill
    {
        public skill333() : base(333, "테라버스트", 80, 100, 0, 26, false, 100, "자신의 색깔에 해당하는 타입들의 피해를 준다")
        {
        }

        public override IEnumerator use_skill(y_color attacker, y_color defender){
            int hit_score = (100-this.accuracy)/5 + Math.Max((this.phy?defender.B:defender.D)-(this.phy?attacker.A:attacker.C),0)/2;
            int hit_dice = rnd.Next(1,21);
            int dicy_point = 0;

            dicy_point += 1;
            
            if(this.efrange > 3){
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
            

            if(dicy_point > 0){
                int hit_dice2 = rnd.Next(1,21);
                if (diceUI == null)
                    diceUI = GameObject.FindObjectOfType<diceRollUI>();
                yield return diceUI.StartCoroutine(diceUI.AdvantageRoll(hit_dice, hit_dice2, hit_score));
                hit_dice = Math.Max(hit_dice,hit_dice2);
            }
            else if (dicy_point == 0){
                if (diceUI == null)
                        diceUI = GameObject.FindObjectOfType<diceRollUI>();
                yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));
            }
            else{
                int hit_dice2 = rnd.Next(1,21);
                if (diceUI == null)
                    diceUI = GameObject.FindObjectOfType<diceRollUI>();
                yield return diceUI.StartCoroutine(diceUI.DisadvantageRoll(hit_dice, hit_dice2, hit_score));
                hit_dice = Math.Min(hit_dice,hit_dice2);
            }

            if(attacker.cc.effect(hit_dice)){
                yield return this.skill_effect(attacker, defender);
                (bool hit, int damage_score) = this.calc_skill(attacker, defender, hit_dice, hit_score);
                yield return defender.damaged(hit, damage_score);
                ApplyAdditional(hit, attacker, defender, damage_score);
            }
            else{
                yield break;
            }
        }

        public override (bool,int) calc_skill(y_color attacker, y_color defender,int hit_dice, int hit_score){
            if(hit_dice==20||(hit_dice!=1&&(hit_score<=hit_dice))){
                Debug.Log("HIT");
                float damage_dice = (float)rnd.Next(1,4);
                float typevs = every_skill.typevs[attacker.type1,defender.type1] * every_skill.typevs[attacker.type1,defender.type2];
                float typevs2 = every_skill.typevs[attacker.type2,defender.type1] * every_skill.typevs[attacker.type2,defender.type2];
                float critical = 1.0f;
                float acbd = (float)Mathf.Max((this.phy?attacker.A:attacker.C) - (this.phy?defender.B:defender.D),0);
                
                if(hit_dice==20){
                    critical = 2.0f;
                    acbd = (float)(this.phy?attacker.A:attacker.C);
                    damage_dice = 4.0f;
                }

                int damage_score = (int)(((float)this.damage)/100.00f * typevs * typevs2 * (acbd + 16 + damage_dice) * critical);
                damage_score = WetherAndField2(damage_score,attacker);

                damage_score = Mathf.Max(damage_score,0);
                Debug.Log($"damage : {this.damage}, typevs {attacker.type1} vs {defender.type1} : {typevs}, acbd {acbd}, damage_dice {damage_dice}, critical {critical}");
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if(defender.hp <= 0){
                    UnityEngine.Object.Destroy(defender.gameObject);
                }
                return (true, damage_score);
            }
            else{
                Debug.Log($"{this.name} MISS");
                return (false, 0);
            }
        }

        public int WetherAndField2(int damage_score, y_color attacker) {
            if(GlobalVariables.weather == 1 && attacker.type1 == 1)
                damage_score = (int)(damage_score * 1.5f);
            else if(GlobalVariables.weather == 1 && attacker.type1 == 2)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.weather == 2 && attacker.type1 == 2)
                damage_score = (int)(damage_score * 1.5f);
            else if (GlobalVariables.weather == 2 && attacker.type1 == 1)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.weather == 3 && attacker.type1 == 12)
                damage_score = (int)(damage_score * 1.5f);
            else if (GlobalVariables.weather == 3 && attacker.type1 == 5)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.weather == 4 && attacker.type1 == 5)
                damage_score = (int)(damage_score * 1.5f);
            else if (GlobalVariables.weather == 4 && attacker.type1 == 12)
                damage_score = (int)(damage_score * 1.5f);

            if(GlobalVariables.field == 1 && attacker.type1 == 3)
                damage_score = (int)(damage_score * 1.3f);
            else if(GlobalVariables.field == 1 && attacker.type1 == 4)
                damage_score = (int)(damage_score * 0.5f);
                
            else if(GlobalVariables.field == 2 && attacker.type1 == 4)
                damage_score = (int)(damage_score * 1.3f);
            else if(GlobalVariables.field == 2 && attacker.type1 == 3)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.field == 3 && attacker.type1 == 10)
                damage_score = (int)(damage_score * 1.3f);
            else if (GlobalVariables.field == 3 && attacker.type1 == 17)
                damage_score = (int)(damage_score * 0.5f);
                
            else if (GlobalVariables.field == 4 && attacker.type1 == 17)
                damage_score = (int)(damage_score * 1.3f);
            else if (GlobalVariables.field == 4 && attacker.type1 == 10)
                damage_score = (int)(damage_score * 0.5f);


            if (GlobalVariables.weather == 1 && attacker.type2 == 1)
                damage_score = (int)(damage_score * 1.5f);
            else if (GlobalVariables.weather == 1 && attacker.type2 == 2)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.weather == 2 && attacker.type2 == 2)
                damage_score = (int)(damage_score * 1.5f);
            else if (GlobalVariables.weather == 2 && attacker.type2 == 1)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.weather == 3 && attacker.type2 == 12)
                damage_score = (int)(damage_score * 1.5f);
            else if (GlobalVariables.weather == 3 && attacker.type2 == 5)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.weather == 4 && attacker.type2 == 5)
                damage_score = (int)(damage_score * 1.5f);
            else if (GlobalVariables.weather == 4 && attacker.type2 == 12)
                damage_score = (int)(damage_score * 1.5f);

            if (GlobalVariables.field == 1 && attacker.type2 == 3)
                damage_score = (int)(damage_score * 1.3f);
            else if (GlobalVariables.field == 1 && attacker.type2 == 4)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.field == 2 && attacker.type2 == 4)
                damage_score = (int)(damage_score * 1.3f);
            else if (GlobalVariables.field == 2 && attacker.type2 == 3)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.field == 3 && attacker.type2 == 10)
                damage_score = (int)(damage_score * 1.3f);
            else if (GlobalVariables.field == 3 && attacker.type2 == 17)
                damage_score = (int)(damage_score * 0.5f);

            else if (GlobalVariables.field == 4 && attacker.type2 == 17)
                damage_score = (int)(damage_score * 1.3f);
            else if (GlobalVariables.field == 4 && attacker.type2 == 10)
                damage_score = (int)(damage_score * 0.5f);

            return damage_score;
        }

        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/GreenDiamond");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            shooting_effect proj = go.GetComponent<shooting_effect>();
            go.GetComponent<SpriteRenderer>().color = attacker.color;
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
        }
    }

}

