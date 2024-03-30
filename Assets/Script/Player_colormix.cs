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
        if(Input.GetKey(KeyCode.Alpha1)){ //min_mix, colormix
                int k=1;
                IfOverlap(k);
        }
        if(Input.GetKey(KeyCode.Alpha2)){  //max_mix, lightmix
                int k=2;
                IfOverlap(k);
        }
        if(Input.GetKey(KeyCode.Alpha3)){ //avg_mix
                int k=3;
                IfOverlap(k);
        }
    }

    private void color_mix(Enemy enemy, int k){
        byte r=0,g=0,b=0;
        Color32 origin_color = this.GetComponent<Player>().color;
        Color32 enemy_color = enemy.color;
        if((k>3) || (k<0)) return;
        if(k==1){
            r = Math.Min(origin_color.r,enemy_color.r);
            g = Math.Min(origin_color.g,enemy_color.g);
            b = Math.Min(origin_color.b,enemy_color.b);
        }
        if(k==2){
            r = Math.Max(origin_color.r,enemy_color.r);
            g = Math.Max(origin_color.g,enemy_color.g);
            b = Math.Max(origin_color.b,enemy_color.b);
        }
        if(k==3){
            r = (byte)(((int)origin_color.r+(int)enemy_color.r)/2);
            g = (byte)(((int)origin_color.g+(int)enemy_color.g)/2);
            b = (byte)(((int)origin_color.b+(int)enemy_color.b)/2);
        }
        Color tmp = new Color32(r,g,b,255);
        this.GetComponent<Player>().color = tmp;
        playerRenderer.color = tmp;
    }
    

    private void IfOverlap(int k){
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
    }
}
