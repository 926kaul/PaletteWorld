using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type22
{
    public class skill233 : monoskill
    {
        public skill233() : base(233, "얼어붙은\n바람", 80, 100, 22, 0, false, 100, "30% 확률로 상대를 동상 상태로 만든다")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
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
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            System.Random rnd = new System.Random();
            int dice = rnd.Next(1, 21);
            if ((dice >= 15) && (defender.cc is ncc) && hit)
            {
                defender.cc = new frz(defender);
            }
            return;
        }
    }
    public class skill244 : monoskill
    {
        public skill244() : base(244, "눈보라", 110, 70, 22, 22, false, 10, "설경에서 첫 명중 굴림이 20이 된다. 모래바람에서는 1이 된다.")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            float duration = 0.5f;              // 전체 연타 시간
            float interval = 0.05f;             // 펀치 간 간격
            float elapsed = 0f;

            Vector3 attackerPos = attacker.transform.position;
            Vector3 defenderPos = defender.transform.position;
            bool arrived = false;

            while (elapsed < duration)
            {
                elapsed += interval;

                // 이펙트 생성
                GameObject prefab = Resources.Load<GameObject>("Prefab/MidwinterIcewind");
                GameObject go = UnityEngine.Object.Instantiate(
                    prefab,
                    attackerPos,
                    Quaternion.identity
                );

                // 약간의 오차를 추가하여 자연스러운 느낌
                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-0.3f, 0.3f),
                    UnityEngine.Random.Range(-0.3f, 0.3f),
                    0f
                );

                shooting_effect proj = go.GetComponent<shooting_effect>();
                proj.target = defenderPos + randomOffset;

                // 마지막 발사에만 도착 감지
                if (elapsed + interval >= duration)
                {
                    proj.onArrive = () => { arrived = true; };
                }

                yield return new WaitForSeconds(interval);
            }

            // 마지막 펀치 도착까지 대기
            yield return new WaitUntil(() => arrived);
        }
        public override IEnumerator use_skill(y_color attacker, y_color defender){

            int hit_score = (100-this.accuracy)/5 + Math.Max((this.phy?defender.B:defender.D)-(this.phy?attacker.A:attacker.C),0)/2;
            int hit_dice = rnd.Next(1,21);
            int dicy_point = 0;

            if((this.type1 == attacker.type1) || (this.type1 == attacker.type2)) //자속보정으로 유리보정
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


            if (GlobalVariables.weather == 4)
                hit_dice = 20;
            if (GlobalVariables.weather == 3)
                hit_dice = 1;
            
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
    }
}