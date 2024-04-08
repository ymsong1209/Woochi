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
                // �ش� �ε����� AbilityTriggerArea�� Ȱ��ȭ
                SkillTriggerAreas[i].SetActive(true);
            }
        }


    }

    /// <summary>
    /// AbilityTriggerAreas ���� ��Ȱ��ȭ��Ų��.
    /// </summary>
    void DeactivateSkillTriggerAreas()
    {
        foreach (var area in SkillTriggerAreas)
        {
            area.SetActive(false);
        }
    }
}
