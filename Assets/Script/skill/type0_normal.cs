using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class type0{
    public class skill111 : monoskill{
        public skill111() : base(111, "튀어오르기", 0, 100, 0, 25, true){
        }
    }
    public class skill211 : monoskill{
        public skill211() : base(211, "메가톤킥", 120, 75, 0, 6, true){
        }
    }
    public class skill311 : monoskill{
        public skill311() : base(311, "아픔나누기", 0, 100, 0, 13, true){
        }

        /*public override IEnumerator calc_skill(y_color attacker, y_color defender){
            System.Random rnd = new System.Random();

            int hit_score = (100-this.accuracy)/5 + Mathf.Max(defender.S-attacker.H,0);
            int hit_dice = rnd.Next(1,21);
            yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));

            if(hit_dice==20||hit_dice!=1||hit_score<=hit_dice){
                Debug.Log("HIT");
                int avg_hp = attacker.hp + defender.hp;
                attacker.hp = Mathf.Min(55 + 3*attacker.H,avg_hp);
                defender.hp = Mathf.Min(55 + 3*defender.H,avg_hp);
            }
            else{
                Debug.Log($"{this.name} MISS");
            }
        }*/
    }
    public class skill121 : monoskill{
        public skill121() : base(121, "베어가르기", 70, 100, 0, 11, true){
        }

        /*public override IEnumerator calc_skill(y_color attacker, y_color defender){
            System.Random rnd = new System.Random();

            int hit_score = (100-this.accuracy)/5 + Mathf.Max(defender.S-attacker.H,0);
            int hit_dice = rnd.Next(1,21);
            yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));

            if(hit_dice>=19||hit_dice!=1||hit_score<=hit_dice){
                Debug.Log("HIT");
                int damage_dice = rnd.Next(1,21);
                if(this.type1==attacker.type1 || this.type1==attacker.type2){
                    int damage_dice2 = rnd.Next(1,21);
                    damage_dice = Mathf.Max(damage_dice,damage_dice2);
                }
                int damage_score = this.damage * Mathf.Max(damage_dice + (this.phy?attacker.A:attacker.C) - (this.phy?defender.B:defender.D),0);
                if(damage_dice==1){
                    damage_score = (int)((float)damage_score * Mathf.Max(every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] - 1,0) / 100);
                }
                else if(damage_dice>=19){
                    damage_score = (int)((float)damage_score * (every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] + 1) / 100);
                }
                else{
                    damage_score = (int)((float)damage_score * (every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] + 1) / 100);
                }
                damage_score = Mathf.Max(damage_score,0);
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if(defender.hp <= 0){
                    Object.Destroy(defender.gameObject);
                }
            }
            else{
                Debug.Log($"{this.name} MISS");
            }
        }*/
    }

    public class skill221 : monoskill{
        public skill221() : base(221, "자폭", 200, 100, 0, 12, true){
        }

        /*public override IEnumerator calc_skill(y_color attacker, y_color defender){
            System.Random rnd = new System.Random();

            int hit_score = (100-this.accuracy)/5 + Mathf.Max(defender.S-attacker.H,0);
            int hit_dice = rnd.Next(1,21);
            yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));
            
            if(hit_dice>=19||hit_dice!=1||hit_score<=hit_dice){
                Debug.Log("HIT");
                int damage_dice = rnd.Next(1,21);
                if(this.type1==attacker.type1 || this.type1==attacker.type2){
                    int damage_dice2 = rnd.Next(1,21);
                    damage_dice = Mathf.Max(damage_dice,damage_dice2);
                }
                int damage_score = this.damage * Mathf.Max(damage_dice + (this.phy?attacker.A:attacker.C) - (this.phy?defender.B:defender.D),0);
                if(damage_dice==1){
                    damage_score = (int)((float)damage_score * Mathf.Max(every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] - 1,0) / 100);
                }
                else if(damage_dice>=19){
                    damage_score = (int)((float)damage_score * (every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] + 1) / 100);
                }
                else{
                    damage_score = (int)((float)damage_score * (every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] + 1) / 100);
                }
                damage_score = Mathf.Max(damage_score,0);
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if(defender.hp <= 0){
                    Object.Destroy(defender.gameObject);
                }
                attacker.hp = 0;
                Object.Destroy(attacker.gameObject);
            }
            else{
                Debug.Log($"{this.name} MISS");
            }
        }*/
    }
    public class skill321 : monoskill{
        public skill321() : base(321, "대폭발", 250, 100, 0, 8, true){
        }

        /*public override IEnumerator calc_skill(y_color attacker, y_color defender){
            System.Random rnd = new System.Random();

            int hit_score = (100-this.accuracy)/5 + Mathf.Max(defender.S-attacker.H,0);
            int hit_dice = rnd.Next(1,21);
            yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));

            if(hit_dice==20||hit_dice!=1||hit_score<=hit_dice){
                Debug.Log("HIT");
                int damage_dice = rnd.Next(1,21);
                if(this.type1==attacker.type1 || this.type1==attacker.type2){
                    int damage_dice2 = rnd.Next(1,21);
                    damage_dice = Mathf.Max(damage_dice,damage_dice2);
                }
                int damage_score = this.damage * Mathf.Max(damage_dice + (this.phy?attacker.A:attacker.C) - (this.phy?defender.B:defender.D),0);
                if(damage_dice==1){
                    damage_score = (int)((float)damage_score * Mathf.Max(every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] - 1,0) / 100);
                }
                else if(damage_dice==20){
                    damage_score = (int)((float)damage_score * (every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] + 1) / 100);
                }
                else{
                    damage_score = (int)((float)damage_score * (every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2] + 1) / 100);
                }
                damage_score = Mathf.Max(damage_score,0);
                Debug.Log($"{this.name} damage {damage_score}");
                defender.hp -= damage_score;
                if(defender.hp <= 0){
                    Object.Destroy(defender.gameObject);
                }
                attacker.hp = 0;
                Object.Destroy(attacker.gameObject);
            }
            else{
                Debug.Log($"{this.name} MISS");
            }
        }*/
    }
    public class skill131 : monoskill{
        public skill131() : base(131, "우웩", 0, 100, 0, 3, true){
        }

        /*public override IEnumerator calc_skill(y_color attacker, y_color defender){
            System.Random rnd = new System.Random();
            int hit_dice = rnd.Next(1,21);
            if(hit_dice!=1){
                Debug.Log("HIT");
                if(defender.cc is ncc){
                    defender.cc = new psn(defender);
                    Debug.Log($"{this.name} psn");
                }
                else{
                    Debug.Log($"{this.name} MISS");
                }
            }
            else{
                Debug.Log($"{this.name} MISS");
            }
        }*/
    }

    public class skill231 : monoskill{
        public skill231() : base(231, "칼춤", 0, 100, 0, 7, true){
        }

        /*public override void calc_skill(y_color attacker, y_color defender){
            System.Random rnd = new System.Random();
            int hit_dice = rnd.Next(1,21);
            if(hit_dice!=1){
                Debug.Log("HIT");
                attacker.A += 4;
                Debug.Log($"{this.name} SUCCESS");
            }
            else{
                Debug.Log($"{this.name} MISS");
            }
        }*/
    }

    public class skill331 : monoskill{
        public skill331() : base(331, "찌릿", 0, 100, 0, 4, true){
        }
        /*public override void calc_skill(y_color attacker, y_color defender){
            System.Random rnd = new System.Random();
            int hit_dice = rnd.Next(1,21);
            if(hit_dice!=1){
                Debug.Log("HIT");
                if(defender.cc is ncc){
                    defender.cc = new par(defender);
                    Debug.Log($"{this.name} par");
                }
                else{
                    Debug.Log($"{this.name} MISS");
                }
            }
            else{
                Debug.Log($"{this.name} MISS");
            }
        }*/
    }


    public class skill222 : monoskill{
        public skill222() : base(222, "몸통박치기", 40, 100, 0, 26, true, 1){
        }
    }
}


