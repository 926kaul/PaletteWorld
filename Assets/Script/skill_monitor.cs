using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using System;

public class skill_monitor : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer render;
    public int monitor_number;
    public int skill_mode; //-1 is inactive, 0 is pending, 1 is using
    public my_color selected;
    void Start()
    {
        string objectName = gameObject.name;
        monitor_number =  int.Parse(Regex.Match(objectName, @"\d+").Value);
        GlobalVariables.skill_monitors[monitor_number] = this;
        render = GetComponent<SpriteRenderer>();
        render.color = new Color32(0,0,0,255);
        render.sortingLayerName = "Background";
        skill_mode = -1;
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
        }
        if (Input.GetMouseButtonDown(0) && skill_mode==1 && selected == Turn.turn_order[0]){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null){
                enemy_color enemy = hit.collider.GetComponent<enemy_color>();
                if(enemy != null){
                    selected.use_skill(enemy,GlobalVariables.selected_skill);
                }
            }
        }
    }

    void OnMouseDown(){
        if(skill_mode==0){
            GlobalVariables.selected_skill = every_skill.get_skill(render.color);
            skill_mode=1;
        }
    }

}
