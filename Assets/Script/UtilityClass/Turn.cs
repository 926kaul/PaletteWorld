using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;

public class Turn{
    public static List<y_color> turn_order = new List<y_color>();
    public static bool Turn_start(){
        Debug.Log("Turn Start");
        bool ans = false;
        turn_order = new List<y_color>();
        int layerMask = LayerMask.GetMask("Default");
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll(new Vector2(0,0), new Vector2(18,18), layerMask);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.GetComponent<y_color>() != null)
            {   
                y_color tmp = collider.GetComponent<y_color>();
                if(tmp is my_color)
                    ans = true;
                turn_order.Add(tmp);
                tmp.skill_locked = false; //턴 시작 시 스킬 잠금 해제
                tmp.distance = 8 + tmp.S/2;
            }
        }
        turn_order.Sort(comparing);
        GameObject.FindObjectOfType<TurnUI>()?.UpdateTurnDisplay();
        return ans;
    }
    public static void Turn_next(y_color done_color){
        turn_order.Remove(done_color);
        turn_order.RemoveAll(item => item == null); //공격을 맞고 destory된 것을 제거
        if(turn_order.Count==0){
            Turn_start();
        }
        else{
            turn_order.Sort(comparing);
        }
        GameObject.FindObjectOfType<TurnUI>()?.UpdateTurnDisplay();

    }
    public static int comparing(y_color x, y_color y){
        Random random = new Random();
        int comparison = y.S.CompareTo(x.S);
        if (comparison == 0)
        {
            return random.Next(-1, 2);
        }
        return comparison;
    }
}

