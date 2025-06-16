using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type2
{
    public class skill113 : monoskill
    {
        public skill113() : base(113, "물대포", 40, 100, 2, 0, false, 100)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/BlueCircle");
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
    public class skill004 : monoskill
    {
        public skill004() : base(004, "하이드로펌프", 110, 85, 2, 2, false, 100)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 start = attacker.transform.position;
            Vector3 end = defender.transform.position;
            Vector3 direction = (end - start);
            float fullLength = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 프리팹 생성: 위치는 attacker
            GameObject prefab = Resources.Load<GameObject>("Prefab/NormalBeam");
            GameObject beam = UnityEngine.Object.Instantiate(prefab, start, Quaternion.Euler(0, 0, angle));

            // 처음엔 길이 0
            beam.transform.localScale = new Vector3(0f, 0.3f, 1f);

            // SpriteRenderer 세팅
            SpriteRenderer sr = beam.GetComponent<SpriteRenderer>();
            sr.color = new Color(0f, 0f, 1f, 1f);
            if (sr != null)
            {
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

                // scale 키우기
                beam.transform.localScale = new Vector3(curLength, 0.3f, 1f);

                // 위치 보정: 중앙 피벗을 고려해 beam의 중심을 start → 중간으로 이동
                beam.transform.position = start + (direction.normalized * curLength / 2f);

                yield return null;
            }

            // 최종 위치/스케일 고정
            beam.transform.localScale = new Vector3(fullLength, 0.3f, 1f);
            beam.transform.position = start + direction.normalized * fullLength / 2f;

            yield return new WaitForSeconds(holdTime);
            UnityEngine.Object.Destroy(beam);
        }
    }
}