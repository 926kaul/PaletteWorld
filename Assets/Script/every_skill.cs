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

public class every_skill : MonoBehaviour{
    public class monoskill{
        public int code;
        public int damage;
        public int accuracy;
        public int type;
        public bool adp;
        public monoskill(int Code, bool Adp, int Damage, int Accuracy, int Type){
            code = Code;
            damage = Damage;
            accuracy = Accuracy;
            type = Type;
            adp = Adp;
        }
    }
    public static monoskill[,,] skillset = new monoskill[5,5,5];
    void Start(){
        skillset[3,0,0] = new monoskill(300,false,40,100,1);
        skillset[0,3,0] = new monoskill(30,false,40,100,2);
        skillset[0,0,3] = new monoskill(3,false,40,100,3);
        typevs[2,1] = 2;
        typevs[3,2] = 2;
        typevs[1,3] = 2;
        typevs[1,1] = 1;
        typevs[2,2] = 1;
        typevs[3,3] = 1;
        typevs[1,2] = 0.5f;
        typevs[2,3] = 0.5f;
        typevs[3,1] = 0.5f;
    }
    public static monoskill get_skill(Color color){
        int[] skill_color = {Mathf.RoundToInt(color.r * 255/64), Mathf.RoundToInt(color.g * 255/64), Mathf.RoundToInt(color.b * 255/64)};
        return skillset[skill_color[0],skill_color[1],skill_color[2]];
    }


    public static float[,] typevs = new float[18,18];
    public static int[,,] color_and_types = new int[3,3,3]
    {
        {
            {-1, 6 , 1},
            {11, 12, 8},
            {3, 7, 4}
        },
        {
            {14, 13, 10},
            {16, 0, 1},
            {19, 3, 4}
        },
        {
            {2, 15, 17},
            {9, 2, 17},
            {5, 5, -1}
        }
    };
    public static int color_to_type(Color color){
        int[] skill_color = {Mathf.RoundToInt(color.r * 2), Mathf.RoundToInt(color.g * 2), Mathf.RoundToInt(color.b * 2)};
        return color_and_types[skill_color[0],skill_color[1],skill_color[2]];
    }
}

