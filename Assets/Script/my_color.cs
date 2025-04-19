using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.U2D.IK;
using TMPro;

public class y_color : MonoBehaviour
{
    public SpriteRenderer render;
    public Color32 color;
    public int H,A,B,C,D,S;
    public int type1, type2;
    public int hp;
    public List<Color> skills = new List<Color>();
    public CC cc;
    public bool skill_locked = false;
    public int distance;

    public void Update_stat(){
        if (render == null)
            render = GetComponent<SpriteRenderer>();
        render.color = color;
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
        if(type1 == 25)
            Destroy(this.gameObject);
        type2 = every_skill.get_skill(color).type2;
        cc = new ncc(this);
    }
    public void use_skill(y_color enemy, monoskill used_skill){
        if (cc.effect()) {
            StartCoroutine(UseSkillRoutine(enemy, used_skill));
        }
    }
    public IEnumerator UseSkillRoutine(y_color enemy, monoskill used_skill)
    {
        yield return used_skill.use_skill(this, enemy);
    }

    private static GameObject damageTextPrefab;
    public virtual IEnumerator damaged(bool hit, int damage_score){
        if (damageTextPrefab == null)
            damageTextPrefab = Resources.Load<GameObject>("Prefab/damage_score");
        // 1. 텍스트 생성
        GameObject dmgTextObj = Instantiate(damageTextPrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);

        // 2. 텍스트 내용 설정
        TextMeshPro text = dmgTextObj.GetComponent<TextMeshPro>();
        if (hit)
            text.text = damage_score.ToString();
        else
            text.text = "MISS";

        // 3. 색상 defender의 색으로 설정
        text.color = this.color;

        // 4. 살짝 떠오르고 사라지기
        float duration = 1.0f;
        Vector3 startPos = dmgTextObj.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 0.5f, 0);

        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            dmgTextObj.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }

        Destroy(dmgTextObj);
        GameObject.FindObjectOfType<TurnUI>()?.UpdateTurnDisplay();
    }

    void OnMouseEnter()
    {
        Monitor.instance?.ShowStatus(this);
    }

    void OnMouseExit()
    {
        Monitor.instance?.Clear();
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
                GlobalVariables.skill_monitors[i].mainText.text = "";
            }
            GlobalVariables.selected_skill = null;
            GlobalVariables.selected_color = null;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            TryEndTurn();
        }
        if(stage_set==1&&(this.transform.position.x < 0 || this.transform.position.x > 18 || this.transform.position.y < 0 || this.transform.position.y > 18)){
            UnityEngine.Object.Destroy(this.gameObject);
            GameObject.FindObjectOfType<TurnUI>()?.UpdateTurnDisplay();
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
            GlobalVariables.selected_color = this;
            for(int i=0; i<4; i++){
                if(skills.Count > i){
                    SpriteRenderer render = GlobalVariables.skill_monitors[i].render;
                    render.color = skills[i];
                    render.sortingLayerName = "Default";
                    render.sortingOrder = 2;
                    GlobalVariables.skill_monitors[i].mainText.text = every_skill.get_skill(skills[i]).name;
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
        if (stage_set == 0)
        {
            Vector3 pos = transform.position;
            // 위치가 stage 내부이고
            if ((pos.x >= 0) && (pos.x <= 18) && (pos.y >= 0) && (pos.y <= 9))
            {
                // 현재 스테이지 위 my_color 수 세기
                Collider2D[] hits = Physics2D.OverlapAreaAll(new Vector2(0, 0), new Vector2(18, 9), LayerMask.GetMask("Default"));
                int count = 0;
                foreach (var col in hits)
                {
                    my_color mc = col.GetComponent<my_color>();
                    if (mc != null && mc.stage_set == 1 && mc != this)
                        count++;
                }
                // 3개 초과면 안 됨
                if (count >= 3)
                {
                    transform.position = original_position; // 되돌리기
                    return;
                }
                // 통과 시 stage에 올리기
                stage_set = 1;
                transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
            }
            else
            {
                // stage 아닌 곳이면 되돌리기
                transform.position = original_position;
            }
        }
    }

    public void Update_skill(){
        color = render.color;
        byte[] skill_color = {CustomSkillIndex(color.r), CustomSkillIndex(color.g), CustomSkillIndex(color.b)};
        if(skills.Count <= GlobalVariables.fibo[level]){
            skills.Add(new Color32(skill_color[0],skill_color[1],skill_color[2],255));
        }
        else{
            // write later
        }
    }
    public static byte CustomSkillIndex(byte value)
    {
        if (value <= 31) return 0;
        else if (value <= 63) return 63;
        else if (value <= 95) return 64;
        else if (value <= 159) return 128;
        else if (value <= 191) return 191;
        else if (value <= 223) return 192;
        else return 255;
    }

    public void TryEndTurn() {
        if (Turn.turn_order.Count > 0 && Turn.turn_order[0] == this) {
            Turn.Turn_next(this);
        }
    }

}