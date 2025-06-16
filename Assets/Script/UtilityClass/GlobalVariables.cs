using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GlobalVariables
{
    public static List<setball> setballs = new List<setball>(new setball[6]);
    public static List<skill_monitor> skill_monitors = new List<skill_monitor>(new skill_monitor[4]);
    public static InTurn selected_skill;
    public static my_color selected_color;
    public static int[] ball_count = { 0, 0, 0, 0 };

    public static y_color unitThatHandledSpace = null; //스페이스 연속 턴종 버그 픽스
    public static bool OathTrue = false; //맹세 스킬 여부

    public static int weather = 0;
    public static int field = 0;
}

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("CoroutineRunner");
                GameObject.DontDestroyOnLoad(go); // 씬 이동 시 파괴되지 않음
                _instance = go.AddComponent<CoroutineRunner>();
            }
            return _instance;
        }
    }
}