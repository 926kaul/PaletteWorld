using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Monitor : MonoBehaviour
{
    public TextMeshPro mainText;
    public diceRollUI diceRollUI;

    public static Monitor instance;

    void Awake()
    {
        instance = this;
    }

    public void ShowStatus(y_color target)
    {
        if (diceRollUI != null && diceRollUI.isRolling) return;

        string stat = $"<size=8><color=#{ColorUtility.ToHtmlStringRGB(target.color)}>#{ColorUtility.ToHtmlStringRGB(target.color)}\n</size>";
        stat += $"<size=6>";
        stat += $"<color=#00C000>H:{target.H} <color=#C00000>A:{target.A} <color=#000040>B:{target.B}\n";
        stat += $"<color=#0000C0>C:{target.C} <color=#400000>D:{target.D} <color=#004000>S:{target.S}\n";
        (Color t1color, string t1) = every_skill.type_code[target.type1];
        (Color t2color, string t2) = every_skill.type_code[target.type2];
        stat += $"<color=#{ColorUtility.ToHtmlStringRGB(t1color)}>{t1} <color=#{ColorUtility.ToHtmlStringRGB(t2color)}>{t2}\n";
        stat += $"<color=#{ColorUtility.ToHtmlStringRGB(target.color)}>HP: {target.hp}/{55+3*target.H}\n";
        stat += $"<color=#{ColorUtility.ToHtmlStringRGB(target.color)}>Distance: {target.distance}\n";

        if (target.cc is not ncc)
        {
            string ccColorCode = ColorUtility.ToHtmlStringRGB(target.cc.cc_color);
            stat += $" / <color=#{ccColorCode}>cc: {target.cc.GetType().Name}</color>";
        }
        stat += "</size>";

        mainText.text = stat;
    }

    public void Clear()
    {
        mainText.text = "";
    }
}

