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
        if(Input.GetKey(KeyCode.Alpha1)){
                int k=1;
                IfOverlap(k);
        }
    }

    private void minus_mix(Enemy enemy){
        Color origin_color = this.GetComponent<Player>().color;
        Color enemy_color = enemy.color;
        float r = Math.Min(origin_color.r,enemy_color.r);
        float g = Math.Min(origin_color.g,enemy_color.g);
        float b = Math.Min(origin_color.b,enemy_color.b);
        Color tmp = new Color(r,g,b);
        this.GetComponent<Player>().color = tmp;
        playerRenderer.color = tmp;
    }

    private void IfOverlap(int k){
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, playerRenderer.size, 0, collisionLayer);
        foreach(Collider2D collider in colliders){
            if(collider.tag=="Enemy"){
                Enemy enemy = collider.GetComponent<Enemy>();
                if(enemy.state==0){
                    if(k==1){
                        minus_mix(enemy);
                    }
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
}
