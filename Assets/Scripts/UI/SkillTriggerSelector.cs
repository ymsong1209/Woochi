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
        DeactivateSkillTriggerAreas();
        bool[] skillRadius = _skill.SkillRadius;
        for (int i = 0; i < skillRadius.Length; i++)
        {
            //현재 살아있는 적/아군에게서만 skilltriggerarea활성화
            if(i<4 && skillRadius[i] && BattleManager.GetInstance.AllyFormation[i])
            {
                SkillTriggerAreas[i].SetActive(true);
            }
            else if(4<=i && i<8 && skillRadius[i] && BattleManager.GetInstance.EnemyFormation[i - 4])
            {
                SkillTriggerAreas[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// AbilityTriggerAreas 전부 비활성화시킨다.
    /// BattleManager의 Event 등록을 위해 의미없는 인자를 넣음
    /// </summary>
    void DeactivateSkillTriggerAreas(BaseCharacter _character = null, bool isEnable = false)
    {
        foreach (var area in SkillTriggerAreas)
        {
            area.SetActive(false);
        }
    }
}
