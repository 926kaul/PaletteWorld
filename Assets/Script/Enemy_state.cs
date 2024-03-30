using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int hp=100;
    [SerializeField] public int state=1;
    public SpriteRenderer EnemyRender;
    public Sprite dead;
    public Color32 color;
    
    void Start(){
        EnemyRender = GetComponent<SpriteRenderer>();
        EnemyRender.color = color;
    }
    void Update()
    {
        if(hp<=0){
            state=0;
        }
    }

    void LateUpdate(){
        if(state==0){
            EnemyRender.sprite = dead;
        }
    }

    public void giveDamage(int damage){
            hp -= damage;
    }
}
