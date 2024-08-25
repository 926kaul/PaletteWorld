using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GlobalVariables
{
    public static List<setball> setballs = new List<setball>(new setball[6]);
    //public static int[] fibo = {1,2,3,4,4,5,5,6,6,6,6,6,7,7,7,7,7,7,7,7,8};
    public static int[] fibo = {1,2,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4};
    public static List<skill_monitor> skill_monitors = new List<skill_monitor>(new skill_monitor[4]);
    public static Dictionary<int,float> Rankchange = new Dictionary<int,float>
    {
        {-6,0.25f},
        {-5,0.29f},
        {-4,0.34f},
        {-3,0.40f},
        {-2,0.50f},
        {-1,0.67f},
        {0,1f},
        {1,1.50f},
        {2,2.00f},
        {3,2.50f},
        {4,3.00f},
        {5,3.50f},
        {6,4.00f}
    };
}