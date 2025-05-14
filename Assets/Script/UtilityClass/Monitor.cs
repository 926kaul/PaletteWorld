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
        stat += $"<color=#{ColorUtility.ToHtmlStringRGB(target.color)}>HP: {target.hp}/{target.full_hp()}\n";
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

    public void ShowSkillInfo(monoskill skill, Color skillColor)
    {
        if (diceRollUI != null && diceRollUI.isRolling) return;

        string skillColorHex = ColorUtility.ToHtmlStringRGB(skillColor);

        string stat = $"<size=8><color=#{skillColorHex}>{skill.name}\n</size>";
        stat += "<size=6>";

        (Color t1color, string t1) = every_skill.type_code[skill.type1];
        (Color t2color, string t2) = every_skill.type_code[skill.type2];

        stat += $"<color=#{ColorUtility.ToHtmlStringRGB(t1color)}>{t1} ";
        stat += $"<color=#{ColorUtility.ToHtmlStringRGB(t2color)}>{t2}\n";

        string powStr = skill.damage.ToString().PadLeft(3, ' ');
        string accStr = skill.accuracy.ToString().PadLeft(3, ' ');
        string rngStr = skill.efrange.ToString().PadLeft(3, ' ');

        stat += $"<color=#808080>Pow: {powStr}  ";
        stat += $"Acc: {accStr}  ";
        stat += $"Rng: {rngStr}   ";

        if (skill.phy)
            stat += $"<color=#800000>Phy\n\n";
        else
            stat += $"<color=#8000FF>Spe\n\n";

        if (!string.IsNullOrEmpty(skill.info))
        {
            stat += $"<size=5><color=#AAAAAA>{skill.info}";
        }

        stat += "</size>";

        mainText.text = stat;
    }
}

