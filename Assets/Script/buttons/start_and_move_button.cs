using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class start_button : MonoBehaviour
{
    public static bool started = false;
    public TextMeshPro mainText;
    public my_color selected;
    
    void Update()
    {
        if (!started)
            mainText.text = "Start";
        else if (GlobalVariables.selected_color != null)
            mainText.text = $"Move<size=40>({GlobalVariables.selected_color.distance})</size>";
        else
            mainText.text = "";
    }
    void OnMouseDown(){
        selected = GlobalVariables.selected_color;
        if(!started && Turn.Turn_start()){
            started = true;
        }
        else if(Turn.turn_order.Count > 0 && selected == Turn.turn_order[0]){
            GlobalVariables.selected_skill = new move();
        }
    }
}

public class move: InTurn{
}
