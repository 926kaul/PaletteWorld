using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUI : MonoBehaviour
{
    public SpriteRenderer render;   // 하이라이트 색상 바꿀 렌더러
    public Sprite EnemySprite;
    public Sprite MySprite;
    public Color activeColor = new Color(0.25f, 0.25f, 0.25f, 1f);   // 턴 가능 시
    public Color inactiveColor = new Color(1f, 1f, 1f, 1f);   // 턴 불가 시
    public Collider2D clickableCollider;       // 클릭 가능할 때만 켜기
    public y_color currentTarget; // 현재 턴 대상
    public List<SpriteRenderer> turnSlots = new List<SpriteRenderer>(); // 1~8번 슬롯들

    void Start()
    {
        for (int i = 0; i < turnSlots.Count; i++)
            turnSlots[i].color = new Color(1, 1, 1, 0); // 투명하게 설정
    }

    void Update()
    {
        currentTarget = null;
        if (Turn.turn_order.Count > 0)
        {
            y_color top = Turn.turn_order[0];
            foreach (my_color mc in GameObject.FindObjectsOfType<my_color>())
            {
                if (mc.stage_set == 1 && mc == top)
                {
                    currentTarget = mc;
                    break;
                }
            }
        }

        // 하이라이트 처리
        if (render != null)
            render.color = (currentTarget != null) ? activeColor : inactiveColor;

        // 클릭 여부 처리
        if (clickableCollider != null)
            clickableCollider.enabled = (currentTarget != null);
    }

    public void UpdateTurnDisplay()
    {
        for (int i = 0; i < turnSlots.Count; i++)
        {
            if (i < Turn.turn_order.Count)
            {
                y_color unit = Turn.turn_order[i];
                turnSlots[i].color = unit.color;

                if (unit is enemy_color){
                    turnSlots[i].sprite = EnemySprite;
                    turnSlots[i].transform.localScale = new Vector3(
                        0.2336362f*0.125f,
                        1.475188f*0.125f,
                        1
                    );
                }
                else if (unit is my_color){
                    turnSlots[i].sprite = MySprite;
                    Vector3 originalScale = turnSlots[i].transform.localScale;
                    float scaleFactor = 2f;
                    turnSlots[i].transform.localScale = new Vector3(
                            0.2336362f,
                            1.475188f,
                            1
                    );
                }
            }
            else
            {
                turnSlots[i].color = new Color(1, 1, 1, 0); // 투명
                turnSlots[i].sprite = null; // 스프라이트 제거
            }
        }
    }
}
