using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public static List<y_color> turn_order = new List<y_color>();
    private static Turn instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 원하면 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (turn_order.Count > 0 && turn_order[0] == null)
        {
            Debug.Log("현재 턴 유닛이 파괴됨. 자동으로 다음 턴으로 넘깁니다.");
            Turn_next(null);
        }
    }

    public static bool Turn_start()
    {
        Debug.Log("Turn Start");
        bool ans = false;
        turn_order = new List<y_color>();
        int layerMask = LayerMask.GetMask("Default");
        GlobalVariables.OathTrue = false;

        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(new Vector2(0, 0), new Vector2(18, 18), layerMask);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.GetComponent<y_color>() != null)
            {
                y_color tmp = collider.GetComponent<y_color>();
                if (tmp is my_color)
                    ans = true;
                turn_order.Add(tmp);
                tmp.skill_locked = false;
                tmp.distance = 8 + tmp.S / 2;
            }
        }

        turn_order.Sort(comparing);
        GameObject.FindObjectOfType<TurnUI>()?.UpdateTurnDisplay();

        if (turn_order.Count > 0)
        {
            SetTransparency(turn_order[0], 0.5f); // 반투명 처리
            if (turn_order[0] is my_color my)
                my.SelectThisUnit();
        }

        return ans;
    }

    public static void Turn_next(y_color done_color)
    {
        if (done_color != null)
            SetTransparency(done_color, 1.0f);

        turn_order.Remove(done_color);
        turn_order.RemoveAll(item => item == null);

        if (turn_order.Count == 0)
        {
            Turn_start();
        }
        else
        {
            turn_order.Sort(comparing);
            SetTransparency(turn_order[0], 0.5f);
        }

        if (turn_order.Count > 0 && turn_order[0] is my_color my)
            my.SelectThisUnit();

        GameObject.FindObjectOfType<TurnUI>()?.UpdateTurnDisplay();
        instance?.StartCoroutine(ClearSpaceNextFrame());
    }

    public static int comparing(y_color x, y_color y)
    {
        System.Random random = new System.Random();
        int comparison = y.S.CompareTo(x.S); // 속도 내림차순
        if (comparison == 0)
            return random.Next(-1, 2); // 속도 같으면 랜덤

        return comparison;
    }

    private static void SetTransparency(y_color target, float alpha)
    {
        if (target != null && target.render != null)
        {
            Color c = target.render.color;
            c.a = alpha;
            target.render.color = c;
        }
    }

    static IEnumerator ClearSpaceNextFrame()
    {
        yield return null;
        GlobalVariables.unitThatHandledSpace = null;
    }
}
