using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.U2D.IK;

public class y_color : MonoBehaviour
{
    public SpriteRenderer render;
    public Color color;
    public int H,A,B,C,D,S;
    public int type1, type2;
    public int hp;
    public List<Color> skills = new List<Color>();
    public CC cc;
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
        type1 = every_skill.color_to_type(color);
        type2 = every_skill.get_skill(color).type2;
        cc = new ncc(this);
    }
    public void use_skill(y_color enemy, monoskill used_skill){
        if (cc.effect()) {
            StartCoroutine(UseSkillRoutine(enemy, used_skill));
        }
        else {
            Turn.Turn_next(this);
        }
    }
    public IEnumerator UseSkillRoutine(y_color enemy, monoskill used_skill)
    {
        yield return used_skill.use_skill(this, enemy);
        Turn.Turn_next(this);
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

}