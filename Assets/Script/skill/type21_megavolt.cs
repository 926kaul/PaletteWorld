using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type21
{
    public class skill332 : monoskill
    {
        public skill332() : base(332, "볼부비부비", 20, 100, 21, 0, true, 2, "상대를 마비 상태로 만든다.")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MegavoltNuzzle");
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
            if (defender.cc is ncc && hit)
            {
                defender.cc = new par(defender);
            }
            return;
        }
    }
    public class skill442 : monoskill
    {
        public skill442() : base(442, "번개", 110, 70, 21, 21, false, 10, "날씨가 비일때 첫 명중 굴림이 20이 된다. 쾌청에서는 1이 된다.")
        {
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

            lr.startWidth = 10.0f;
            lr.endWidth = 3.0f;
            lr.SetPosition(0, attacker.transform.position);
            lr.SetPosition(1, defender.transform.position);

            lr.material = new Material(Shader.Find("Sprites/Default"));
            Color trueYellow = new Color(1f, 1f, 0.5f, 1f);
            lr.material.color = trueYellow;

            // 시각 효과: 점점 사라짐
            float duration = 0.8f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float t = 1f - elapsed / duration;
                lr.startWidth = 0.3f * t;
                lr.endWidth = 0.15f * t;

                Color flicker = new Color(1f, 1f, 0.5f, t);
                lr.startColor = flicker;
                lr.endColor = flicker;

                yield return null;
            }

            GameObject.Destroy(go);
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


            if (GlobalVariables.weather == 2)
                hit_dice = 20;
            if (GlobalVariables.weather == 1)
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