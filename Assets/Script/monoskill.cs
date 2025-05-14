using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;
using System;
using Unity.Burst.CompilerServices;
using UnityEngine.Rendering.Universal.Internal;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

public class InTurn{}

public class monoskill : InTurn{
    public int code;
    public string name;
    public int damage;
    public int accuracy;
    public int type1;
    public int type2;
    public bool phy;
    public int efrange;
    public string info;
    public monoskill(int Code, string Name, int Damage, int Accuracy, int Type1, int Type2, bool Phy, int Efrange = 100, string Info = ""){
        code = Code;
        name = Name;
        damage = Damage;
        accuracy = Accuracy;
        type1 = Type1;
        type2 = Type2;
        phy = Phy;
        efrange = Efrange;
        info = Info;
    }

    protected System.Random rnd = new System.Random();
    public virtual IEnumerator use_skill(y_color attacker, y_color defender){

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
        

        if(dicy_point > 0){
            int hit_dice2 = rnd.Next(1,21);
            if (diceUI == null)
                diceUI = GameObject.FindObjectOfType<diceRollUI>();
            yield return diceUI.StartCoroutine(diceUI.AdvantageRoll(hit_dice, hit_dice2, hit_score));
            hit_dice = Math.Max(hit_dice,hit_dice2);
        }
        else if (dicy_point == 0){
            if (diceUI == null)
                    diceUI = GameObject.FindObjectOfType<diceRollUI>();
            yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));
        }
        else{
            int hit_dice2 = rnd.Next(1,21);
            if (diceUI == null)
                diceUI = GameObject.FindObjectOfType<diceRollUI>();
            yield return diceUI.StartCoroutine(diceUI.DisadvantageRoll(hit_dice, hit_dice2, hit_score));
            hit_dice = Math.Min(hit_dice,hit_dice2);
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

    public virtual bool skill_availablity(y_color attacker, y_color defender){
        float dist = Vector3.Distance(attacker.transform.position, defender.transform.position);
        return dist <= this.efrange;
    }

    public virtual IEnumerator skill_effect(y_color attacker, y_color defender){
        yield break;
    }

    public diceRollUI diceUI;
    public virtual (bool,int) calc_skill(y_color attacker, y_color defender,int hit_dice, int hit_score){
        if(hit_dice==20||(hit_dice!=1&&(hit_score<=hit_dice))){
            Debug.Log("HIT");
            float damage_dice = (float)rnd.Next(1,4);
            float typevs = every_skill.typevs[this.type1,defender.type1] * every_skill.typevs[this.type1,defender.type2];
            float critical = 1.0f;
            float acbd = (float)Mathf.Max((this.phy?attacker.A:attacker.C) - (this.phy?defender.B:defender.D),0);
            
            if(hit_dice==20){
                critical = 2.0f;
                acbd = (float)(this.phy?attacker.A:attacker.C);
                damage_dice = 4.0f;
            }

            int damage_score = (int)(((float)this.damage)/100.00f * typevs * (acbd + 16 + damage_dice) * critical);

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

    public virtual void ApplyAdditional(bool hit, y_color attacker, y_color defender, int damage_score){
        // nothing for no additional skill
    }

    public virtual IEnumerator react_skill(y_color attacker, y_color defender){

        int hit_score = (100-this.accuracy)/5 + Math.Max((this.phy?defender.B:defender.D)-(this.phy?attacker.A:attacker.C),0)/4;
        int hit_dice = rnd.Next(1,21);

        if(attacker.cc is ncc){
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