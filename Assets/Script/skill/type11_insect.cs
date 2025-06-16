using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type11
{
    public class skill121 : monoskill
    {
        public skill121() : base(121, "벌레먹기", 60, 100, 11, 0, true, 1)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/BeetleBite");
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
    public class skill020 : monoskill
    {
        public skill020() : base(020, "메가폰", 120, 85, 11, 11, true, 9, "적을 관통하여 공격한다. 적중한 적은 모두 피해를 입는다")
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
                    ApplyAdditional(hit, attacker, target, damage_score);
                }
            }
            else
            {
                yield break;
            }

        }
        public IEnumerator skill_effect_with_result(y_color attacker, y_color defender, Action<List<y_color>> callback)
        {
            Vector3 start = attacker.transform.position;

            GameObject prefab = Resources.Load<GameObject>("Prefab/BeetleDrill");
            GameObject go = UnityEngine.Object.Instantiate(prefab, start, prefab.transform.rotation);

            // 발사체 이동 설정
            var proj = go.GetComponent<shooting_effect_endless>();
            proj.target = defender.transform.position;

            // 충돌 대상 필터용 정보
            Type targetType = (attacker is my_color) ? typeof(enemy_color) : typeof(my_color);
            HashSet<y_color> alreadyHit = new HashSet<y_color>();
            List<y_color> hitTargets = new List<y_color>();

            // TriggerListener2D 연결
            TriggerListener2D listener = go.GetComponent<TriggerListener2D>();
            if (listener == null)
                listener = go.AddComponent<TriggerListener2D>();

            listener.onTriggerEnter = (Collider2D col) =>
            {
                y_color unit = col.GetComponent<y_color>();
                if (unit != null && unit != attacker && unit.GetType() == targetType && !alreadyHit.Contains(unit))
                {
                    alreadyHit.Add(unit);
                    hitTargets.Add(unit);
                }
            };

            // 발사체가 파괴될 때 리스트 넘김
            proj.onDestroyCallback = () =>
            {
                callback(hitTargets);
            };

            yield return new WaitUntil(() => go == null);
        }
    }
}