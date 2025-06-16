using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type4
{
    public class skill331 : monoskill
    {
        public skill331() : base(331, "전기쇼크", 40, 100, 4, 0, false)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/YellowThunder");
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
    
    public class skill440 : monoskill{
        public skill440() : base(440, "10만볼트", 90, 100, 4, 4, false, 100, "10% 확률로 상대를 마비 상태로 만든다"){
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

            lr.material = new Material(Shader.Find("Sprites/Default"));
            Color trueYellow = new Color(1f, 1f, 0f, 1f);
            lr.material.color = trueYellow;

            // 시각 효과: 점점 사라짐
            float duration = 0.4f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                float t = 1f - elapsed / duration;
                lr.startWidth = 0.3f * t;
                lr.endWidth = 0.15f * t;

                Color flicker = new Color(1f, 1f, 0f, t); // 붉은 불빛, 점점 투명해짐
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
                defender.cc = new par(defender);
            }
            return;
        }
    }
}