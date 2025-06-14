using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type24{
    public class skill132 : monoskill
    {
        public skill132() : base(132, "최면술", 0, 60, 24, 0, false, 2, "광역(2), 자신을 제외한 주변 색깔들을 잠듦 상태로 만든다.")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            yield return Explosion.Exploding(
                prefabPath: "Prefab/MysticHypnosis",
                position: attacker.transform.position,
                growDuration: 0.45f,
                fadeDuration: 0.15f,
                maxScale: 2f,
                startAlpha: 0.1f,
                endAlpha: 0.8f,
                startColor: new Color(0f, 0f, 0f),
                endColor: new Color(0f, 1.0f, 0.5f)
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
                    if (Vector3.Distance(attacker.transform.position, unit.transform.position) <= 2f)
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
            
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score){
            if(defender.cc is ncc && hit){
                defender.cc = new slp(defender);
            }
            return;
        }
    }
    public class skill142 : monoskill{
        public skill142() : base(142, "트릭룸", 0, 100, 24, 0, false, 100, "스테이지 상의 모든 색깔들의 속도를 반전시킨다."){
        }
        public override IEnumerator use_skill(y_color attacker, y_color defender){

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
                yield return diceUI.StartCoroutine(diceUI.AdvantageRoll(hit_dice, hit_dice2, 1));
                hit_dice = Math.Max(hit_dice,hit_dice2);
            }
            else if (dicy_point == 0){
                if (diceUI == null)
                        diceUI = GameObject.FindObjectOfType<diceRollUI>();
                yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, 1));
            }
            else{
                int hit_dice2 = rnd.Next(1,21);
                if (diceUI == null)
                    diceUI = GameObject.FindObjectOfType<diceRollUI>();
                yield return diceUI.StartCoroutine(diceUI.DisadvantageRoll(hit_dice, hit_dice2, 1));
                hit_dice = Math.Min(hit_dice,hit_dice2);
            }


            if(attacker.cc.effect(hit_dice) && hit_dice > 1){
                yield return this.skill_effect(attacker, defender);
                y_color[] allUnits = GameObject.FindObjectsOfType<y_color>();
                foreach (y_color unit in allUnits)
                {
                    Vector3 pos = unit.transform.position;
                    if (pos.x >= 0 && pos.x <= 18 && pos.y >= 0 && pos.y <= 18){
                        unit.S = 15 - unit.S;
                    }
                }
            }
            else{
                yield break;
            }

        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            Camera mainCam = Camera.main;
            if (mainCam == null) yield break;

            Transform camTransform = mainCam.transform;
            Vector3 center = camTransform.position + camTransform.forward * 5f; // 회전 중심점

            float duration = 2f;
            float speed = 360f / duration;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float angle = speed * Time.deltaTime;
                camTransform.RotateAround(center, camTransform.right, angle);  // ← 이 부분을 X축으로 변경
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        public override bool skill_availablity(y_color attacker, y_color defender){
            return true;
        }
    }
}