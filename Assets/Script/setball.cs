using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class setball : MonoBehaviour
{
    public SpriteRenderer render;
    public int setball_number;
    void Start(){
        string objectName = gameObject.name;
        setball_number =  int.Parse(Regex.Match(objectName, @"\d+").Value);
        GlobalVariables.setballs[setball_number] = this;
    }
}
public class GlobalVariables : MonoBehaviour
{
    public static List<setball> setballs = new List<setball>(new setball[6]);
    //public static int[] fibo = {1,2,3,4,4,5,5,6,6,6,6,6,7,7,7,7,7,7,7,7,8};
    public static int[] fibo = {1,2,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4};
    public static List<skill_monitor> skill_monitors = new List<skill_monitor>(new skill_monitor[4]);
}