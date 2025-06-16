using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type1{
    public class skill311 : monoskill{
        public skill311() : base(311, "불꽃세례", 40, 100, 1, 0, false, 100){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/RedTriangle");
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
    public class skill400 : monoskill{
        public skill400() : base(400, "화염방사", 90, 100, 1, 1, false, 100, "10% 확률로 상대를 화상 상태로 만든다"){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefab/FireBeam");
            GameObject go = GameObject.Instantiate(prefab);
            LineRenderer lr = go.GetComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.alignment = LineAlignment.View;
            lr.widthCurve = new AnimationCurve(
                new Keyframe(0, 1.0f),
                new Keyframe(1, 1.0f)
            );

            lr.startWidth = 5.0f;
            lr.endWidth = 3.0f;
            lr.SetPosition(0, attacker.transform.position);
            lr.SetPosition(1, defender.transform.position);

            // 시각 효과: 점점 사라짐
            float duration = 0.4f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float t = 1f - elapsed / duration;
                lr.startWidth = 0.3f * t;
                lr.endWidth = 0.15f * t;

                Color flicker = new Color(1f, 0.4f, 0f, t); // 붉은 불빛, 점점 투명해짐
                lr.startColor = flicker;
                lr.endColor = flicker;

                yield return null;
            }

            GameObject.Destroy(go);
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score){
            System.Random rnd = new System.Random();
            int dice = rnd.Next(1,21);
            if((dice >= 19)&&(defender.cc is ncc)&&hit){
                defender.cc = new brn(defender);
            }
            return;
        }
    }
    public class skill300 : monoskill{
        public skill300() : base(300, "블레이즈킥", 85, 90, 1, 6, true, 3, "10% 확률로 상대를 화상 상태로 만든다"){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/FireKick");
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
            System.Random rnd = new System.Random();
            int dice = rnd.Next(1,21);
            if((dice >= 19)&&(defender.cc is ncc)&&hit){
                defender.cc = new brn(defender);
            }
            return;
        }
    }
    public class skill310 : monoskill{
        public skill310() : base(310, "불꽃튀기기", 80, 100, 1, 12, false, 100, "30% 확률로 상대를 화상 상태로 만든다"){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/FireRock");
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
            if((dice >= 15)&&(defender.cc is ncc)&&hit){
                defender.cc = new brn(defender);
            }
            return;
        }
    }
    public class skill301 : monoskill{
        public skill301() : base(301, "도깨비불", 0, 85, 1, 13, false, 100, "상대를 화상 상태로 만든다"){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/FireGhost");
            GameObject go = UnityEngine.Object.Instantiate(
                prefab,
                attacker.transform.position,
                prefab.transform.rotation
            );

            shooting_effect_norotation proj = go.GetComponent<shooting_effect_norotation>();
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score){
            if((defender.cc is ncc)&&hit){
                defender.cc = new brn(defender);
            }
            return;
        }
    }
    public class skill401 : monoskill{
        public skill401() : base(401, "질투의불꽃", 70, 100, 1, 17, false, 100, "상대의 스탯이 변화되었다면, 화상 상태로 만든다"){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/FireJealousy");
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
            int Red = Mathf.RoundToInt(defender.color.r * 255);
            int Green = Mathf.RoundToInt(defender.color.g * 255);
            int Blue = Mathf.RoundToInt(defender.color.b * 255);
            int H = Green%16;
            int A = Red%16;
            int C = Blue%16;
            int B = 15-C;
            int D = 15-A;
            int S = 15-H;

            if(H==defender.H&&A==defender.A&&C==defender.C&&B==defender.B&&D==defender.D&&S==defender.S){
                return;
            }
            if((defender.cc is ncc)&&hit){
                defender.cc = new brn(defender);
            }
            return;
        }
    }

    public class skill411 : monoskill{
        public skill411() : base(411, "블래스트번", 150, 100, 1, 18, false, 100, "사용 후 반동 상태가 된다"){
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender){
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/FireBlast");
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
            attacker.cc = new rbd(attacker);
            return;
        }
    }

    public class skill410 : monoskill{
        public skill410() : base(410, "분화", 150, 100, 1, 8, false, 100, "광역 전체(3), 자신의 HP가 적을 수록 위력이 떨어진다"){
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
                startColor: new Color(192f / 255f, 0f, 0f),
                endColor: new Color(1f, 63f / 255f, 0f)
            );
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
                bool hit; int damage_score;
                y_color[] allUnits = GameObject.FindObjectsOfType<y_color>();
                foreach (y_color unit in allUnits)
                {
                    if (Vector3.Distance(defender.transform.position, unit.transform.position) <= 3f)
                    {   
                        hit_score = (100-this.accuracy)/5 + Math.Max((this.phy?unit.B:unit.D)-(this.phy?attacker.A:attacker.C),0)/2;
                        (hit, damage_score) = this.calc_skill(attacker, unit, hit_dice, hit_score);
                        CoroutineRunner.Instance.StartCoroutine(unit.damaged(hit, damage_score));
                        ApplyAdditional(hit, attacker, unit, damage_score);
                    }
                }
                
            }
            else{
                yield break;
            }
        }

        public override (bool,int) calc_skill(y_color attacker, y_color defender,int hit_dice, int hit_score){
            if(hit_dice==20||(hit_dice!=1&&(hit_score<=hit_dice))){
                Debug.Log("HIT");
                float damage_dice = (float)rnd.Next(1,4);
                float typevs = every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2];
                float critical = 1.0f;
                float acbd = (float)Mathf.Max((this.phy?attacker.A:attacker.C) - (this.phy?defender.B:defender.D),0);
                float tmp_damage = (float)(this.damage * attacker.hp / attacker.full_hp());
                
                if(hit_dice==20){
                    critical = 2.0f;
                    acbd = (float)(this.phy?attacker.A:attacker.C);
                    damage_dice = 4.0f;
                }

                int damage_score = (int)(((float)tmp_damage)/100.00f * typevs * (acbd + 16 + damage_dice) * critical);
                damage_score = WetherAndField(damage_score);

                damage_score = Mathf.Max(damage_score,0);
                Debug.Log($"damage : {tmp_damage}, typevs {this.type1} vs {defender.type1} : {typevs}, acbd {acbd}, damage_dice {damage_dice}, critical {critical}");
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
        
    }
}