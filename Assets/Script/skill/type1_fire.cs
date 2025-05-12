using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type1{
    public class skill311 : monoskill{
        public skill311() : base(311, "불꽃세례", 40, 100, 1, 0, false){
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
}