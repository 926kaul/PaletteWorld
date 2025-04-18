using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;
using Unity.Burst.CompilerServices;
using UnityEditor.UI;

public class InTurn{}

public class monoskill : InTurn{
    public int code;
    public string name;
    public int damage;
    public int accuracy;
    public int type1;
    public int type2;
    public bool phy;
    public monoskill(int Code, string Name, int Damage, int Accuracy, int Type1, int Type2, bool Phy){
        code = Code;
        name = Name;
        damage = Damage;
        accuracy = Accuracy;
        type1 = Type1;
        type2 = Type2;
        phy = Phy;
    }

    System.Random rnd = new System.Random();
    public virtual IEnumerator use_skill(y_color attacker, y_color defender){

        int hit_score = (100-this.accuracy)/5 + Math.Max(Math.Min(defender.S,defender.H)-attacker.H,0);
        int hit_dice = rnd.Next(1,21);
        if (diceUI == null)
            diceUI = GameObject.FindObjectOfType<diceRollUI>();
        yield return diceUI.StartCoroutine(diceUI.Roll(hit_dice, hit_score));

        yield return this.skill_effect(attacker, defender);
        (bool hit, int damage_score) = this.calc_skill(attacker, defender, hit_dice, hit_score);
        yield return defender.damaged(hit, damage_score);
    }

    public virtual IEnumerator skill_effect(y_color attacker, y_color defender){
        yield break;
    }

    public diceRollUI diceUI;
    public virtual (bool,int) calc_skill(y_color attacker, y_color defender,int hit_dice, int hit_score){
        if(hit_dice==20||(hit_dice!=1&&(hit_score<=hit_dice))){
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
                UnityEngine.Object.Destroy(defender.gameObject);
            }
            return (true, damage_score);
        }
        else{
            Debug.Log($"{this.name} MISS");
            return (false, 0);
        }
    }

}

public class every_skill : MonoBehaviour{
    public static float[,] typevs = new float[27,27];
    public static float[,] sub_typevs = new float[18,18]
    {
        // Normal
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.0f, 1.0f, 1.0f, 0.5f, 1.0f },
        // Fire
        { 1.0f, 0.5f, 0.5f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 2.0f, 1.0f },
        // Water
        { 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f },
        // Grass
        { 1.0f, 0.5f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 0.5f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f },
        // Electric
        { 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f },
        // Ice
        { 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f },
        // Fighting
        { 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 0.5f, 0.5f, 2.0f, 0.0f, 1.0f, 2.0f, 2.0f, 0.5f },
        // Poison
        { 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.0f, 2.0f },
        // Ground
        { 1.0f, 2.0f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.0f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f },
        // Flying
        { 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f },
        // Psychic
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.5f, 1.0f },
        // Bug
        { 1.0f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 2.0f, 0.5f, 0.5f },
        // Rock
        { 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f },
        // Ghost
        { 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f },
        // Dragon
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.0f },
        // Dark
        { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f },
        // Steel
        { 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 0.5f, 2.0f },
        // Fairy
        { 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 0.5f, 1.0f },
        
    };
    void Start(){
        Dictionary<int,int> adv_type = new Dictionary<int,int>(){
            {18,1},{19,2},{20,3},{21,4},{22,5},{23,17},{24,10}
        };
        for (int i = 0; i < 18; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                typevs[i, j] = sub_typevs[i, j];
            }
        }
        for(int i=0; i<18; i++){
            for(int j=18; j<25; j++){
                typevs[i, j] = sub_typevs[i, adv_type[j]];
                typevs[j, i] = sub_typevs[adv_type[j], i];
            }
        }
        for(int i=18; i<25; i++){
            for(int j=18; j<25; j++){
                typevs[i, j] = sub_typevs[adv_type[i], adv_type[j]];
            }
        }
        for(int i=0;i<25;i++){
            typevs[i, 25] = 2.0f;  //black and white is 25
            typevs[i, 26] = 1.0f; // None type is 26
        }

        skillset[1,3,1,1] = new type1.skill311();
        skillset[2,1,1,3] = new type2.skill113();
        skillset[3,1,3,1] = new type3.skill131();

        skillset[0,1,1,1] = new type0.skill111();
        skillset[0,2,1,1] = new type0.skill211();
        skillset[0,3,1,1] = new type0.skill311();
        skillset[0,1,2,1] = new type0.skill121();
        skillset[0,2,2,1] = new type0.skill221();
        skillset[0,3,2,1] = new type0.skill231();
        skillset[0,1,3,1] = new type0.skill131();
        skillset[0,2,3,1] = new type0.skill231();
        skillset[0,3,3,1] = new type0.skill331();

    }

    public start_button start_button;
    void Update(){
        my_color selected = GlobalVariables.selected_color;
        if (Input.GetMouseButtonDown(0) && start_button.started && Turn.turn_order.Count > 0 && selected == Turn.turn_order[0]){
            if (GlobalVariables.selected_skill is monoskill  && !selected.skill_locked){
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null){
                    enemy_color enemy = hit.collider.GetComponent<enemy_color>();
                    if(enemy != null){
                        selected.skill_locked = true;
                        selected.use_skill(enemy,(monoskill)GlobalVariables.selected_skill);
                    }
                }
            }
            else if (GlobalVariables.selected_skill is move)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = 0f;
                if(mouseWorldPos.x < 0 || mouseWorldPos.x > 18 || mouseWorldPos.y < 0 || mouseWorldPos.y > 18){
                    return;
                }
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, LayerMask.GetMask("Default"));
                if (hit != null && hit.GetComponent<y_color>() != null)
                    return;
                
                float dist = Vector2.Distance(selected.transform.position, mouseWorldPos);

                if (selected.distance >= dist)
                {
                    selected.transform.position = mouseWorldPos;
                    selected.distance -= Mathf.RoundToInt(dist);  // 거리 감소 (정수 처리)
                }
                else
                {
                    Debug.Log("too far to move");
                }
            }
        }
        
    }
    public static int[,,] color_and_types = new int[3,3,3]
    {
        {
            {25, 14, 2},
            {11, 16, 9},
            {3, 24, 5}
        },
        {
            {6, 13, 15},
            {12, 0, 19},
            {7, 20, 22}
        },
        {
            {1, 10, 17},
            {8, 18, 23},
            {4, 21, 25}
        }
    };
    public static int color_to_type(Color color){
        int[] skill_color = {Mathf.RoundToInt(color.r * 2), Mathf.RoundToInt(color.g * 2), Mathf.RoundToInt(color.b * 2)};
        return color_and_types[skill_color[0],skill_color[1],skill_color[2]];
    }

    public static monoskill[,,,] skillset = new monoskill[20,5,5,5];
    public static monoskill get_skill(Color color){
        int[] skill_color = {Mathf.RoundToInt(color.r * 255/64), Mathf.RoundToInt(color.g * 255/64), Mathf.RoundToInt(color.b * 255/64)};
        int type1 = color_to_type(color);
        return skillset[type1, skill_color[0],skill_color[1],skill_color[2]];
    }

    public static (Color,string)[] type_code = new (Color,string)[27]
    {   
        (new Color(255,255,255,255),"Normal"), // 0
        (new Color(255,0,0,255),"Fire"), // 1
        (new Color(0,0,255,255),"Water"), // 2
        (new Color(0,255,0,255),"Grass"), // 3
        (new Color(255,255,0,255),"Electric"), // 4
        (new Color(0,255,255,255),"Ice"), // 5
        (new Color(255,0,255,255),"Fighting"), // 6
        (new Color(255,128,0,255),"Poison"), // 7
        (new Color(255,128,128,255),"Ground"), //8
        (new Color(0,128,255,255),"Flying"), // 9
        (new Color(128,0,255,255),"Psychic"), // 10
        (new Color(0,128,0,255),"Insect"), // 11
        (new Color(128,128,0,255),"Rock"), // 12
        (new Color(128,0,128,255),"Ghost"), // 13
        (new Color(0,0,128,255),"Dragon"), // 14
        (new Color(128,0,255),"Dark"), // 15
        (new Color(0,128,128,255),"Steel"), // 16
        (new Color(255,0,128,255),"Fairy"), // 17
        (new Color(255,128,128,255),"Fire"), // 18
        (new Color(128,128,255,255),"Water"), // 19
        (new Color(128,255,128,255),"Grass"), // 20
        (new Color(255,255,128,255),"Electric"), // 21
        (new Color(128,255,255,255),"Ice"), // 22
        (new Color(255,128,255,255),"Fairy"), // 23
        (new Color(0,255,128,255),"Psychic"), // 24
        (new Color(0,0,0,255),"BW"), // 25
        (new Color(255,255,255),"None"), // 26
    };
}

