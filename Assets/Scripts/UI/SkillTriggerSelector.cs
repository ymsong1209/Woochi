using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTriggerSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> SkillTriggerAreas = new List<GameObject>();

    private void Start()
    {
        DeactivateSkillTriggerAreas();
        BattleManager.GetInstance.OnCharacterTurnStart += DeactivateSkillTriggerAreas;
    }


    public void Activate(BaseSkill _skill)
    {
        Debug.Log("Activate Called");
        DeactivateSkillTriggerAreas();
        bool[] skillRadius = _skill.SkillRadius;
        for (int i = 0; i < skillRadius.Length; i++)
        {
            if (skillRadius[i])
            {
                Debug.Log("SetActive Called");
                // 해당 인덱스의 AbilityTriggerArea를 활성화
                SkillTriggerAreas[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// AbilityTriggerAreas 전부 비활성화시킨다.
    /// BattleManager의 Event 등록을 위해 의미없는 인자를 넣음
    /// </summary>
    void DeactivateSkillTriggerAreas(BaseCharacter _character = null)
    {
        foreach (var area in SkillTriggerAreas)
        {
            area.SetActive(false);
        }
    }
}
