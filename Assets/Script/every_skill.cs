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
        skillset[4,3,3,1] = new type4.skill331();
        skillset[5,1,3,3] = new type5.skill133();
        skillset[6,2,1,1] = new type6.skill211();
        skillset[7,2,3,1] = new type7.skill231();
        skillset[8,3,2,1] = new type8.skill321();
        skillset[9,1,2,3] = new type9.skill123();
        skillset[10,3,1,3] = new type10.skill313();
        skillset[11,1,1,2] = new type11.skill121();
        skillset[12,2,2,1] = new type12.skill221();
        skillset[13,1,2,2] = new type13.skill212();
        skillset[14,1,1,2] = new type14.skill112();
        skillset[15,2,2,3] = new type15.skill223();
        skillset[16,1,2,2] = new type16.skill122();
        skillset[17,3,1,3] = new type17.skill313();
        skillset[18,3,2,2] = new type18.skill322();
        skillset[19,2,2,3] = new type19.skill223();
        skillset[20,2,3,2] = new type20.skill232();
        skillset[21,3,3,2] = new type21.skill332();
        skillset[22,2,3,3] = new type22.skill233();
        skillset[23,3,2,3] = new type23.skill323();
        skillset[24,1,3,2] = new type24.skill132();


        /*skillset[0,1,1,1] = new type0.skill111();
        skillset[0,2,1,1] = new type0.skill211();
        skillset[0,3,1,1] = new type0.skill311();
        skillset[0,1,2,1] = new type0.skill121();
        skillset[0,2,2,1] = new type0.skill221();
        skillset[0,3,2,1] = new type0.skill231();
        skillset[0,1,3,1] = new type0.skill131();
        skillset[0,2,3,1] = new type0.skill231();
        skillset[0,3,3,1] = new type0.skill331();*/

        normalskill[0] = new type0.skill222();
        normalskill[1] = new type1.skill311();
        normalskill[2] = new type2.skill113();
        normalskill[3] = new type3.skill131();
        normalskill[4] = new type4.skill331();
        normalskill[5] = new type5.skill133();
        normalskill[6] = new type6.skill211();
        normalskill[7] = new type7.skill231();
        normalskill[8] = new type8.skill321();
        normalskill[9] = new type9.skill123();
        normalskill[10] = new type10.skill313();
        normalskill[11] = new type11.skill121();
        normalskill[12] = new type12.skill221();
        normalskill[13] = new type13.skill212();
        normalskill[14] = new type14.skill112();
        normalskill[15] = new type15.skill223();
        normalskill[16] = new type16.skill122();
        normalskill[17] = new type17.skill313();

        normalskill[18] = new type18.skill322();
        normalskill[19] = new type19.skill223();
        normalskill[20] = new type20.skill232();
        normalskill[21] = new type21.skill332();
        normalskill[22] = new type22.skill233();
        normalskill[23] = new type23.skill323();
        normalskill[24] = new type24.skill132();

        normalskill[25] = new type0.skill222();
        normalskill[26] = new type0.skill222();
    }

    public start_button start_button;
    void Update(){
        my_color selected = GlobalVariables.selected_color;
        if (Input.GetMouseButtonDown(0) && start_button.started && Turn.turn_order.Count > 0 && selected == Turn.turn_order[0]){
            if (GlobalVariables.selected_skill is monoskill  && !selected.skill_locked){
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null){
                    enemy_color enemy = hit.collider.GetComponent<enemy_color>();
                    if(enemy != null && ((monoskill)GlobalVariables.selected_skill).skill_availablity(selected, enemy)){
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
                bool blocked = false;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(mouseWorldPos, 0.3f, LayerMask.GetMask("Default"));
                foreach (Collider2D c in colliders)
                {
                    if (c.GetComponent<y_color>() != null && c.gameObject != selected.gameObject)
                    {
                        blocked = true;
                        break;
                    }
                }
                if(blocked) return;

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
            else if (GlobalVariables.selected_skill is ball && !selected.skill_locked)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null){
                    enemy_color enemy = hit.collider.GetComponent<enemy_color>();
                    if(enemy != null){
                        selected.skill_locked = true;
                        StartCoroutine(((ball)GlobalVariables.selected_skill).use_ball(selected, enemy));
                    }
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
    public static int color_to_type(Color32 color){
        int[] skill_color = {CustomTypeIndex(color.r), CustomTypeIndex(color.g), CustomTypeIndex(color.b)};
        return color_and_types[skill_color[0],skill_color[1],skill_color[2]];
    }
    public static int CustomTypeIndex(byte value)
    {
        if (value <= 63) return 0;
        else if (value <= 191) return 1;
        else return 2;
    }

    public static monoskill[,,,] skillset = new monoskill[27,5,5,5];
    public static monoskill[] normalskill = new monoskill[27];
    public static monoskill get_skill(Color32 color){
        int[] skill_color = {CustomSkillIndex(color.r), CustomSkillIndex(color.g),CustomSkillIndex(color.b)};
        int type1 = color_to_type(color);
        monoskill tmp = skillset[type1, skill_color[0],skill_color[1],skill_color[2]];
        Debug.Log($"skillset[{type1},{skill_color[0]},{skill_color[1]},{skill_color[2]}]");
        if(tmp == null){
            tmp = normalskill[type1];
        }
        return tmp;
    }
    public static int CustomSkillIndex(byte value)
    {
        if (value <= 31) return 0;
        else if (value <= 95) return 1;
        else if (value <= 159) return 2;
        else if (value <= 223) return 3;
        else return 4;
    }

    public static (Color,string)[] type_code = new (Color,string)[27]
    {   
        (new Color32(255,255,255,255),"Normal"), // 0
        (new Color32(255,0,0,255),"Fire"), // 1
        (new Color32(0,0,255,255),"Water"), // 2
        (new Color32(0,255,0,255),"Grass"), // 3
        (new Color32(255,255,0,255),"Electric"), // 4
        (new Color32(0,255,255,255),"Ice"), // 5
        (new Color32(255,0,255,255),"Fighting"), // 6
        (new Color32(255,128,0,255),"Poison"), // 7
        (new Color32(255,128,128,255),"Ground"), //8
        (new Color32(0,128,255,255),"Flying"), // 9
        (new Color32(128,0,255,255),"Psychic"), // 10
        (new Color32(0,128,0,255),"Insect"), // 11
        (new Color32(128,128,0,255),"Rock"), // 12
        (new Color32(128,0,128,255),"Ghost"), // 13
        (new Color32(0,0,128,255),"Dragon"), // 14
        (new Color32(128,0,255,255),"Dark"), // 15
        (new Color32(0,128,128,255),"Steel"), // 16
        (new Color32(255,0,128,255),"Fairy"), // 17
        (new Color32(255,128,128,255),"Magma"), // 18
        (new Color32(128,128,255,255),"Marine"), // 19
        (new Color32(128,255,128,255),"Meadow"), // 20
        (new Color32(255,255,128,255),"Megavolt"), // 21
        (new Color32(128,255,255,255),"Midwinter"), // 22
        (new Color32(255,128,255,255),"Melody"), // 23
        (new Color32(0,255,128,255),"Mystic"), // 24
        (new Color32(0,0,0,255),"BW"), // 25
        (new Color32(255,255,255,255),"None"), // 26
    };
}

