using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_button : MonoBehaviour
{
    public static bool started = false;
    public SpriteRenderer render;
    void OnMouseDown(){
        if(!started){
            started = true;
            every_skill.Turn_start();
            render = GetComponent<SpriteRenderer>();
            render.sortingLayerName = "Background";
            render.sortingOrder = -1;
            render.color = new Color(255,255,255,255);
        }
    }
}
