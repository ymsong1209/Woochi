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
                // �ش� �ε����� AbilityTriggerArea�� Ȱ��ȭ
                SkillTriggerAreas[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// AbilityTriggerAreas ���� ��Ȱ��ȭ��Ų��.
    /// BattleManager�� Event ����� ���� �ǹ̾��� ���ڸ� ����
    /// </summary>
    void DeactivateSkillTriggerAreas(BaseCharacter _character = null)
    {
        foreach (var area in SkillTriggerAreas)
        {
            area.SetActive(false);
        }
    }
}
