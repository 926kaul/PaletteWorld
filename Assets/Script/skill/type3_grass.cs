using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type3
{
    public class skill131 : monoskill
    {
        public skill131() : base(131, "나뭇잎", 40, 100, 3, 0, true)
        {
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
            proj.target = defender.transform.position;
            proj.onArrive = () => { arrived = true; };
            yield return new WaitUntil(() => arrived);
        }
    }
    public class skill040 : monoskill
    {
        public skill040() : base(040, "기가드레인", 75, 100, 3, 3, false, 10, "준 피해의 절반만큼 체력을 회복한다.")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 start = defender.transform.position; // 💡 시작점: defender
            Vector3 end = attacker.transform.position;   // 💡 도착점: attacker
            Vector3 direction = (end - start);
            float fullLength = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 프리팹 생성: 위치는 defender
            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalBeam");
            GameObject beam = UnityEngine.Object.Instantiate(prefab, start, Quaternion.Euler(0, 0, angle));

            beam.transform.localScale = new Vector3(0f, 0.3f, 1f); // 처음 길이 0

            SpriteRenderer sr = beam.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(0f, 1f, 0f, 1f);
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

                beam.transform.localScale = new Vector3(curLength, 0.3f, 1f);

                // 위치 보정: defender 기준 → attacker 방향으로 자라도록
                beam.transform.position = start + direction.normalized * curLength / 2f;

                yield return null;
            }

            // 최종 상태
            beam.transform.localScale = new Vector3(fullLength, 0.3f, 1f);
            beam.transform.position = start + direction.normalized * fullLength / 2f;

            yield return new WaitForSeconds(0.2f);
            UnityEngine.Object.Destroy(beam);
        }


        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score){
            if(hit)
                attacker.hp = Math.Min(attacker.hp + damage_score/2, attacker.full_hp());
        }
    }
}