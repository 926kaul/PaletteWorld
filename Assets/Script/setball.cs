using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class setball : MonoBehaviour
{
    public SpriteRenderer setball_sprite;
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
}