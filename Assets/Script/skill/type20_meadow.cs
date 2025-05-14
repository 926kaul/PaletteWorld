using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type20{
    public class skill232 : monoskill{
        public skill232() : base(322, "풀의 맹세", 50, 100, 20, 0, false, 100, "이번 라운드에 맹세 스킬이 사용되었다면,\n <color=#C08080>불의 맹세</color>와 <color=#8080C0>물의 맹세</color>를 추가로 사용한다."){
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

                if(GlobalVariables.OathTrue){
                    if (attacker == null || defender == null) yield break;
                    monoskill MagmaOath = new type18.skill322();
                    yield return MagmaOath.skill_effect(attacker, defender);
                    (hit, damage_score) = MagmaOath.calc_skill(attacker, defender, hit_dice, hit_score);
                    yield return defender.damaged(hit, damage_score);

                    if(attacker == null || defender == null) yield break;
                    monoskill MarineOath = new type19.skill223();
                    yield return MarineOath.skill_effect(attacker, defender);
                    (hit, damage_score) = MarineOath.calc_skill(attacker, defender, hit_dice, hit_score);
                    yield return defender.damaged(hit, damage_score);
                }

                ApplyAdditional(hit, attacker, defender, damage_score);
            }
            else{
                yield break;
            }
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MeadowOath");
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
            GlobalVariables.OathTrue = true;
        }

        public override IEnumerator react_skill(y_color attacker, y_color defender){

            int hit_score = (100-this.accuracy)/5 + Math.Max((this.phy?defender.B:defender.D)-(this.phy?attacker.A:attacker.C),0)/4;
            int hit_dice = rnd.Next(1,21);

            if(attacker.cc is ncc){
                yield return this.skill_effect(attacker, defender);
                (bool hit, int damage_score) = this.calc_skill(attacker, defender, hit_dice, hit_score);
                yield return defender.damaged(hit, damage_score);

                if(GlobalVariables.OathTrue){
                    if (attacker == null || defender == null) yield break;
                    monoskill MagmaOath = new type18.skill322();
                    yield return MagmaOath.skill_effect(attacker, defender);
                    (hit, damage_score) = MagmaOath.calc_skill(attacker, defender, hit_dice, hit_score);
                    yield return defender.damaged(hit, damage_score);

                    if(attacker == null || defender == null) yield break;
                    monoskill MarineOath = new type19.skill223();
                    yield return MarineOath.skill_effect(attacker, defender);
                    (hit, damage_score) = MarineOath.calc_skill(attacker, defender, hit_dice, hit_score);
                    yield return defender.damaged(hit, damage_score);
                }

                ApplyAdditional(hit, attacker, defender, damage_score);
            }
            else{
                yield break;
            }
        }
    }
    public class skill242 : monoskill{
        public skill242() : base(242, "트릭플라워", 35, 100, 20, 0, true, 100, "무조건 급소에 명중한다."){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MeadowFlower");
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
        public override IEnumerator use_skill(y_color attacker, y_color defender){

            int hit_score = (100-this.accuracy)/5 + Math.Max((this.phy?defender.B:defender.D)-(this.phy?attacker.A:attacker.C),0)/2;
            int hit_dice = 20;
            if (diceUI == null)
                diceUI = GameObject.FindObjectOfType<diceRollUI>();
            yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));

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