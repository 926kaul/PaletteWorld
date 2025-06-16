using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type12
{
    public class skill221 : monoskill
    {
        public skill221() : base(221, "돌떨구기", 40, 100, 12, 0, true, 100)
        {
        }
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            bool arrived = false;

            GameObject prefab = Resources.Load<GameObject>("Prefab/RockRock");
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
    public class skill220 : monoskill{
        public skill220() : base(220, "스톤에지", 100, 80, 12, 12, true, 8, "급소에 맞기 쉽다"){
        }
        public override (bool,int) calc_skill(y_color attacker, y_color defender,int hit_dice, int hit_score){
            if(hit_dice>=19||(hit_dice!=1&&(hit_score<=hit_dice))){
                Debug.Log("HIT");
                float damage_dice = (float)rnd.Next(1,4);
                float typevs = every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2];
                float critical = 1.0f;
                float acbd = (float)Mathf.Max((this.phy?attacker.A:attacker.C) - (this.phy?defender.B:defender.D),0);
                
                if(hit_dice>=19){
                    critical = 2.0f;
                    acbd = (float)(this.phy?attacker.A:attacker.C);
                    damage_dice = 4.0f;
                }

                int damage_score = (int)(((float)this.damage)/100.00f * typevs * (acbd + 16 + damage_dice) * critical);
                damage_score = WetherAndField(damage_score);

                damage_score = Mathf.Max(damage_score,0);
                Debug.Log($"damage : {this.damage}, typevs {this.type1} vs {defender.type1} : {typevs}, acbd {acbd}, damage_dice {damage_dice}, critical {critical}");
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
        public override IEnumerator skill_effect(y_color attacker, y_color defender)
        {
            float duration = 0.4f;
            float interval = 0.1f;
            float elapsed = 0f;

            Vector3 attackerPos = attacker.transform.position;
            Vector3 defenderPos = defender.transform.position;
            bool arrived = false;

            while (elapsed < duration)
            {
                elapsed += interval;

                // 이펙트 생성
                GameObject prefab = Resources.Load<GameObject>("Prefab/GreenDiamond");
                prefab.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0f, 1f);
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
    }
}