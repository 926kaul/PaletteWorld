using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class autoskill_button : MonoBehaviour
{
    public TurnUI turnUI;
    public TextMeshPro mainText;
    void Update()
    {
        if (GlobalVariables.selected_color != null && Turn.turn_order.Count > 0 && GlobalVariables.selected_color == Turn.turn_order[0])
            mainText.text = "Auto";
        else
            mainText.text = "";
    }
    void OnMouseDown()
    {
        // 1. 유효한 상황인지 검사
        if (turnUI.currentTarget != null &&
            GlobalVariables.selected_color != null &&
            Turn.turn_order.Count > 0 &&
            GlobalVariables.selected_color == Turn.turn_order[0])
        {
            my_color my = GlobalVariables.selected_color as my_color;

            if (my != null && my.skills.Count > 0)
            {
                // 2. 모든 가능한 (스킬, 타겟) 조합 수집
                y_color[] allUnits = GameObject.FindObjectsOfType<y_color>();
                List<enemy_color> possibleTargets = new List<enemy_color>();
                foreach (y_color unit in allUnits)
                {
                    if (unit is enemy_color enemy && enemy != my)
                        possibleTargets.Add(enemy);
                }

                List<(Color, enemy_color)> usableCombos = new List<(Color, enemy_color)>();
                foreach (Color skillColor in my.skills)
                {
                    monoskill skill = every_skill.get_skill(skillColor);
                    foreach (enemy_color target in possibleTargets)
                    {
                        if (skill.skill_availablity(my, target))
                        {
                            usableCombos.Add((skillColor, target));
                        }
                    }
                }

                // 3. 가능한 조합이 없으면 종료
                if (usableCombos.Count == 0)
                {
                    Debug.Log("No valid skill-target pair available.");
                    return;
                }

                // 4. 무작위 조합 선택 및 시전
                var (selectedSkillColor, selectedTarget) = usableCombos[Random.Range(0, usableCombos.Count)];
                monoskill selectedSkill = every_skill.get_skill(selectedSkillColor);
                my.use_skill(selectedTarget, selectedSkill);
            }
        }
    }
}
