using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class type15
{
    public class skill213 : monoskill
    {
        public skill213() : base(213, "물기", 40, 100, 15, 0, true, 2)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/DarkBite");
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
    public class skill204 : monoskill
    {
        public skill204() : base(204, "깨물어\n부수기", 80, 100, 15, 15, true, 3, "20% 확률로 상대의 방어를 5 떨어뜨린다")
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            Vector3 direction = (defender.transform.position - attacker.transform.position).normalized;
            Vector3 vertical = new Vector3(0f, 1f, 0f);  // 위아래 방향

            float offset = 1.5f;
            float speed = 25f;
            float stopDistanceFromDefender = 0.5f;
            float holdDuration = 0.1f;
            float maxTime = 1.0f;

            // 시작 위치: 위, 아래
            Vector3 topStart = defender.transform.position + vertical * offset;
            Vector3 bottomStart = defender.transform.position - vertical * offset;

            GameObject prefab = Resources.Load<GameObject>("Prefab/DarkCrunch");
            GameObject top = UnityEngine.Object.Instantiate(prefab, topStart, Quaternion.identity);
            GameObject bottom = UnityEngine.Object.Instantiate(prefab, bottomStart, Quaternion.identity);

            // 아래쪽 턱은 y축 반전 (상하 뒤집기)
            Vector3 originalScale = bottom.transform.localScale;
            bottom.transform.localScale = new Vector3(originalScale.x, -Mathf.Abs(originalScale.y), originalScale.z);

            // 위치 변수
            Vector3 topPos = topStart;
            Vector3 bottomPos = bottomStart;

            Vector3 topDir = (defender.transform.position - topStart).normalized;
            Vector3 bottomDir = (defender.transform.position - bottomStart).normalized;

            float elapsed = 0f;
            while ((Vector3.Distance(topPos, defender.transform.position) > stopDistanceFromDefender ||
                    Vector3.Distance(bottomPos, defender.transform.position) > stopDistanceFromDefender) &&
                elapsed < maxTime)
            {
                float step = speed * Time.deltaTime;
                topPos += topDir * step;
                bottomPos += bottomDir * step;

                top.transform.position = topPos;
                bottom.transform.position = bottomPos;

                elapsed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(holdDuration);
            UnityEngine.Object.Destroy(top);
            UnityEngine.Object.Destroy(bottom);
        }
        public override void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score)
        {
            System.Random rnd = new System.Random();
            int dice = rnd.Next(1,21);
            if((dice >= 17)&&hit){
                defender.B = Math.Max(defender.B - 5, 0);
            }
            return;

        }
    }
}