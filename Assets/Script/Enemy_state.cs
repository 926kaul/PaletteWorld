using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int hp=100;
    [SerializeField] public int state=1;
    public SpriteRenderer spriter;
    public Sprite dead;
    public Color color;
    
    void Start(){
        spriter = GetComponent<SpriteRenderer>();
        color = new Color(255,0,0);
    }
    void Update()
    {
        if(hp<=0){
            state=0;
        }
    }

    void LateUpdate(){
        if(state==0){
            spriter.sprite = dead;
        }
    }

    public void giveDamage(int damage){
            hp -= damage;
    }
}
