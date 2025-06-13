using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor;



public class WeatherAndField : MonoBehaviour
{
    public SpriteRenderer field;
    public SpriteRenderer weather;
    public void Update()
    {
        if (GlobalVariables.weather == 0)
        {
            weather.color = new Color32(0, 0, 0, 0);
        }
        else if (GlobalVariables.weather == 1)
        {
            weather.color = new Color32(255, 128, 128, 128);
        }
        else if (GlobalVariables.weather == 2)
        {
            weather.color = new Color32(128, 128, 255, 128);
        }
        else if (GlobalVariables.weather == 3)
        {
            weather.color = new Color32(192, 192, 128, 128);
        }
        else if (GlobalVariables.weather == 4)
        {
            weather.color = new Color(128, 255, 255, 192);
        }

        if (GlobalVariables.field == 0)
        {
            field.color = new Color32(0, 0, 0, 255);
        }
        else if (GlobalVariables.field == 1)
        {
            field.color = new Color32(0, 64, 0, 255);
        }
        else if (GlobalVariables.field == 2)
        {
            field.color = new Color32(64, 64, 0, 255);
        }
        else if (GlobalVariables.field == 3)
        {
            field.color = new Color32(64, 32, 64, 255);
        }
        else if (GlobalVariables.field == 4)
        {
            field.color = new Color32(128, 64, 128, 255);
        }
    }
}