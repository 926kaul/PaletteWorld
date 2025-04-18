using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class turnend_button : MonoBehaviour
{
    public TurnUI turnUI;
    public TextMeshPro mainText;
    void Update()
    {
        if (GlobalVariables.selected_color != null && Turn.turn_order.Count > 0 && GlobalVariables.selected_color == Turn.turn_order[0])
            mainText.text = "End";
        else
            mainText.text = "";
    }
    void OnMouseDown()
    {
        if (turnUI.currentTarget != null)
            Turn.Turn_next(turnUI.currentTarget);
    }
}
