using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
            Destroy(this.gameObject);
        }
    }

    public void giveDamage(int damage){
            hp -= damage;
    }

    public void captured(AA ball){
        int ball_state = ball.state;
        int randomValue = UnityEngine.Random.Range(0, 100);
        bool result = (randomValue > hp);

        StartCoroutine(CaptureOrNot());

        IEnumerator CaptureOrNot(){
            Color32 tmp_color = color;
            gameObject.layer = LayerMask.NameToLayer("out");
            this.GetComponent<EnemyMove>().speed=0;
            yield return new WaitForSeconds(1.0f);

            if (result)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player_mix>().color_mix(tmp_color, ball_state);
                state = 0;
                gameObject.layer = LayerMask.NameToLayer("Default");
                Destroy(gameObject);
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
                this.GetComponent<EnemyMove>().speed=4;
            }
            if(ball!=null) Destroy(ball.gameObject);
        }
    }
}
