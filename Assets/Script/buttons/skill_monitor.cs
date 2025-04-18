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
    void Start()
    {
        string objectName = gameObject.name;
        monitor_number =  int.Parse(Regex.Match(objectName, @"\d+").Value);
        GlobalVariables.skill_monitors[monitor_number] = this;
        render = GetComponent<SpriteRenderer>();
        render.color = new Color32(0,0,0,255);
        render.sortingLayerName = "Background";
    }

    void OnMouseDown(){
        GlobalVariables.selected_skill = every_skill.get_skill(render.color);
    }

}
