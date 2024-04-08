using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTriggerSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> SkillTriggerAreas = new List<GameObject>();

    private void Start()
    {
        DeactivateSkillTriggerAreas();
    }

    public void Activate(BaseSkill _skill)
    {

        DeactivateSkillTriggerAreas();
        bool[] skillRadius = _skill.SkillRadius;
        for (int i = 0; i < skillRadius.Length; i++)
        {
            if (skillRadius[i])
            {
                // 해당 인덱스의 AbilityTriggerArea를 활성화
                SkillTriggerAreas[i].SetActive(true);
            }
        }


    }

    /// <summary>
    /// AbilityTriggerAreas 전부 비활성화시킨다.
    /// </summary>
    void DeactivateSkillTriggerAreas()
    {
        foreach (var area in SkillTriggerAreas)
        {
            area.SetActive(false);
        }
    }
}
