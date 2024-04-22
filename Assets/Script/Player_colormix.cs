using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_mix : MonoBehaviour
{
    public LayerMask collisionLayer; // layer for collision test
    private SpriteRenderer playerRenderer;
    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void color_mix(Color32 enemy_color, int k){
        byte r=0,g=0,b=0;
        Color32 origin_color = this.GetComponent<Player>().color;
        if((k>4) || (k<2)) return;
        if(k==2){
            r = Math.Min(origin_color.r,enemy_color.r);
            g = Math.Min(origin_color.g,enemy_color.g);
            b = Math.Min(origin_color.b,enemy_color.b);
        }
        if(k==3){
            r = Math.Max(origin_color.r,enemy_color.r);
            g = Math.Max(origin_color.g,enemy_color.g);
            b = Math.Max(origin_color.b,enemy_color.b);
        }
        if(k==4){
            r = (byte)(((int)origin_color.r+(int)enemy_color.r)/2);
            g = (byte)(((int)origin_color.g+(int)enemy_color.g)/2);
            b = (byte)(((int)origin_color.b+(int)enemy_color.b)/2);
        }
        Color tmp = new Color32(r,g,b,255);
        this.GetComponent<Player>().color = tmp;
        playerRenderer.color = tmp;
    }
    

    /*private void IfOverlap(int k){
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, playerRenderer.size, 0, collisionLayer);
        foreach(Collider2D collider in colliders){
            if(collider.tag=="Enemy"){
                Enemy enemy = collider.GetComponent<Enemy>();
                if(enemy.state==0){
                    color_mix(enemy,k);
                    Destroy(enemy.gameObject);
                }
            }
        }
    }*/
}
