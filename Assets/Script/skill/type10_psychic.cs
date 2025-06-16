using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type10
{
    public class skill313 : monoskill
    {
        public skill313() : base(313, "염동력", 40, 100, 10, 0, false)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/PinkPsychic");
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
    public class skill404 : monoskill
    {
        public skill404() : base(404, "사이코\n키네시스", 90, 100, 10, 10, false, 10, "염력을 상대를 끌어와 공격한다")
        {
        }
        public override IEnumerator use_skill(y_color attacker, y_color defender)
        {

            int hit_score = (100 - this.accuracy) / 5 + Math.Max((this.phy ? defender.B : defender.D) - (this.phy ? attacker.A : attacker.C), 0) / 2;
            int hit_dice = rnd.Next(1, 21);
            int dicy_point = 0;

            if ((this.type1 == attacker.type1) || (this.type1 == attacker.type2)) //자속보정으로 유리보정
                dicy_point += 1;

            // 원거리 불리 보정 x

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
                yield return defender.damaged(hit, damage_score);
                ApplyAdditional(hit, attacker, defender, damage_score);
            }
            else
            {
                yield break;
            }
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 start = attacker.transform.position;
            Vector3 end = defender.transform.position;
            Vector3 direction = (end - start).normalized;
            float fullLength = Vector3.Distance(start, end);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Beam 생성
            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalBeam");
            GameObject beam = UnityEngine.Object.Instantiate(prefab, start, Quaternion.Euler(0, 0, angle));
            beam.transform.localScale = new Vector3(0f, 0.3f, 1f);

            SpriteRenderer sr = beam.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(1f, 0f, 1f, 1f);
                sr.sortingLayerName = "Effect";
                sr.sortingOrder = 5;
            }

            // 빔 자라나는 연출
            float growTime = 0.2f;
            float elapsed = 0f;

            while (elapsed < growTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / growTime);
                float curLength = t * fullLength;

                beam.transform.localScale = new Vector3(curLength, 0.3f, 1f);
                beam.transform.position = start + direction * (curLength / 2f);

                yield return null;
            }

            // defender 끌기 준비
            Collider2D defCol = defender.GetComponent<Collider2D>();
            if (defCol != null) defCol.enabled = false;

            Vector3 target = start + direction * 0.2f; // attacker 앞 0.2 위치
            Vector3 initialDefPos = defender.transform.position;
            float pullTime = 0.3f;
            elapsed = 0f;

            while (elapsed < pullTime)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / pullTime);

                // 1. defender 끌기
                defender.transform.position = Vector3.Lerp(initialDefPos, target, t);

                // 2. beam 줄이기
                float curLength = fullLength * (1f - t);
                beam.transform.localScale = new Vector3(curLength, 0.3f, 1f);
                beam.transform.position = start + direction * (curLength / 2f);

                yield return null;
            }

            // 최종 위치 보정
            defender.transform.position = target;
            if (defCol != null) defCol.enabled = true;

            // 빔 제거
            UnityEngine.Object.Destroy(beam);
            yield return new WaitForSeconds(0.1f);
        }

    }
}