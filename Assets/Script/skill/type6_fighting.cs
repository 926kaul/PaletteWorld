using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type6
{
    public class skill211 : monoskill
    {
        public skill211() : base(211, "마하펀치", 40, 100, 6, 0, true, 3)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/MahaPunch");
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

    public class skill200 : monoskill
    {
        public skill200() : base(200, "인파이트", 120, 100, 6, 6, true, 3, "사용 후 방어와 특수방어가 절반이 됩니다") { }

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
                GameObject prefab = Resources.Load<GameObject>("Prefab/FightingPunches");
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

        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            if (hit)
            {
                attacker.B /= 2;
                attacker.D /= 2;
            }
        }
    }
    

}