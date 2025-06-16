using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type9
{
    public class skill123 : monoskill
    {
        public skill123() : base(123, "쪼기", 40, 100, 9, 0, true, 2)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/FlyingBeak");
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
    public class skill024 : monoskill
    {
        public skill024() : base(024, "공중날기", 90, 95, 9, 9, true, 10, "상대에게 날아달려들며, 스피드가 5 증가한다")
        {
        }
        public override IEnumerator use_skill(y_color attacker, y_color defender){

            int hit_score = (100-this.accuracy)/5 + Math.Max((this.phy?defender.B:defender.D)-(this.phy?attacker.A:attacker.C),0)/2;
            int hit_dice = rnd.Next(1,21);
            int dicy_point = 0;

            if((this.type1 == attacker.type1) || (this.type1 == attacker.type2)) //자속보정으로 유리보정
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
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            // 충돌 방지
            Collider2D col = attacker.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            // 진행 방향 계산
            Vector3 startPos = attacker.transform.position;
            Vector3 toDefender = (defender.transform.position - startPos).normalized;

            // 회전 각도 계산
            float angle = Mathf.Atan2(toDefender.y, toDefender.x) * Mathf.Rad2Deg;
            attacker.transform.rotation = Quaternion.Euler(0, 0, angle-90);

            // 날개 프리팹 생성 및 부착
            GameObject wingL = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefab/FlyingWingLeft"), attacker.transform);
            GameObject wingR = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefab/FlyingWingRight"), attacker.transform);

            
            // 부모 스케일
            Vector3 parentScale = attacker.transform.lossyScale;

            // wingL: 프리팹 원래 localScale 기준 보정
            Vector3 baseScaleL = wingL.transform.localScale;
            Vector3 correctedScaleL = new Vector3(
                baseScaleL.x / parentScale.x,
                baseScaleL.y / parentScale.y,
                baseScaleL.z / parentScale.z
            );
            wingL.transform.localScale = correctedScaleL;

            // wingR: 프리팹 원래 localScale 기준 보정
            Vector3 baseScaleR = wingR.transform.localScale;
            Vector3 correctedScaleR = new Vector3(
                baseScaleR.x / parentScale.x,
                baseScaleR.y / parentScale.y,
                baseScaleR.z / parentScale.z
            );
            wingR.transform.localScale = correctedScaleR;

            // 날개 위치 (로컬 좌표 기준)
            wingL.transform.localPosition = new Vector3(-0.5f, 0.2f, 0f);
            wingR.transform.localPosition = new Vector3(0.5f, 0.2f, 0f);

            // 도착 위치 계산 (defender보다 0.2 덜 도착)
            Vector3 targetPos = defender.transform.position - toDefender * 0.2f;
            float speed = 10f;
            float threshold = 0.05f;

            // 이동
            while (Vector3.Distance(attacker.transform.position, targetPos) > threshold)
            {
                attacker.transform.position += toDefender * speed * Time.deltaTime;
                yield return null;
            }

            attacker.transform.position = targetPos;

            // 날개 제거
            UnityEngine.Object.Destroy(wingL);
            UnityEngine.Object.Destroy(wingR);

            // attacker 회전 원복
            attacker.transform.rotation = Quaternion.identity;

            // 충돌 다시 활성화
            if (col != null) col.enabled = true;

            yield return new WaitForSeconds(0.2f);
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            attacker.S += Mathf.Min(31, attacker.S + 5);
        }

    }
}