using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class y_color : MonoBehaviour
{
    public SpriteRenderer render;
    public Color color;
    public int H,A,B,C,D,S;
    public int type;
    public int hp;
    public List<Color> skills = new List<Color>();
    // Start is called before the first frame update
    public void Update_stat(){
        color = render.color;
        int Red = Mathf.RoundToInt(color.r * 255);
        int Green = Mathf.RoundToInt(color.g * 255);
        int Blue = Mathf.RoundToInt(color.b * 255);
        H = Green%16;
        A = Red%16;
        C = Blue%16;
        B = 15-C;
        D = 15-A;
        S = 15-H;
        type = every_skill.color_to_type(color);
    }

    
}

public class my_color : y_color
{
    public int stage_set = 0; // 0 is in ball, 1 is in stage
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 original_position;
    public int level=1;
    // Start is called before the first frame update
    void Start(){
        render = GetComponent<SpriteRenderer>();
        color = render.color;
        Update_stat();
        hp = 55 + 3*H;
        Update_skill();
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                for(int i=0; i<4; i++){
                    SpriteRenderer render = GlobalVariables.skill_monitors[i].render;
                    render.color = new Color(255,255,255,255);
                    render.sortingLayerName = "Background";
                    render.sortingOrder = 0;
                    GlobalVariables.skill_monitors[i].selected = null;
                    GlobalVariables.skill_monitors[i].skill_mode = -1;
                }
        }
    }
    void OnMouseDown(){
        if(stage_set==0){
            Vector3 mousePosition = Input.mousePosition;
            original_position = transform.position;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            offset = transform.position - mousePosition;
            isDragging = true;
        }
        if(stage_set==1){
            for(int i=0; i<4; i++){
                if(skills.Count > i){
                    SpriteRenderer render = GlobalVariables.skill_monitors[i].render;
                    render.color = skills[i];
                    render.sortingLayerName = "Default";
                    render.sortingOrder = 2;
                    GlobalVariables.skill_monitors[i].skill_mode = 0;
                    GlobalVariables.skill_monitors[i].selected = this;
                }
            }
        }
    }
    void OnMouseDrag(){
        if (isDragging){
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }
    void OnMouseUp()
    {
        isDragging = false;
        if(stage_set==0){
            if((transform.position.x>=0)&&(transform.position.x<=18)&&(transform.position.y>=0)&&(transform.position.y<=9)){
                stage_set = 1;
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
            }
            else{
                transform.position = original_position;
            }
        }
    }

    void Update_skill(){
        color = render.color;
        float[] skill_color = {Mathf.Round(color.r * 255/64)/4, Mathf.Round(color.g * 255/64)/4, Mathf.Round(color.b * 255/64)/4};
        if(skills.Count <= GlobalVariables.fibo[level]){
            skills.Add(new Color(skill_color[0],skill_color[1],skill_color[2],255));
        }
        else{
            // write later
        }
    }

    public void use_skill(y_color enemy, every_skill.monoskill used_skill){
        System.Random rnd = new System.Random();

        int hit_score = (100-used_skill.accuracy)/5 + Mathf.Max(enemy.H-(used_skill.adp?this.A:this.C),0) - ((this.type==used_skill.type)?5:0);
        int hit_dice = rnd.Next(1,21);
        if(hit_dice==20||hit_dice!=1||hit_score<=hit_dice){
            Debug.Log("HIT");
            int damage_dice = rnd.Next(1,21);
            if(every_skill.typevs[used_skill.type, enemy.type]!=1){
                int damage_dice2 = rnd.Next(1,21);
                if(every_skill.typevs[used_skill.type, enemy.type]==2){
                    damage_dice = Mathf.Max(damage_dice,damage_dice2);
                }
                else{
                    damage_dice = Mathf.Min(damage_dice,damage_dice2);
                }
                Debug.Log($"{damage_dice},{damage_dice2}");
            }
            int damage_score = used_skill.damage * Mathf.Max(damage_dice + (used_skill.adp?this.A:this.C) - (used_skill.adp?enemy.B:enemy.D),0) / 100;
            if(every_skill.typevs[used_skill.type, enemy.type]==0){
                damage_score = 0;
            }
            if(damage_dice==20){
                damage_score = used_skill.damage * (damage_dice + (used_skill.adp?this.A:this.C)) * 2 /100;
            }
            Debug.Log($"{damage_score} damage");
            enemy.hp -= damage_score;
            if(enemy.hp <= 0){
                Destroy(enemy.gameObject);
            }
        }
        else{
            Debug.Log("MISS");
        }
        Turn.Turn_next(this);
}

}