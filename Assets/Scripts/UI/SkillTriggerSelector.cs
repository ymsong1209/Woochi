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
            if(skillRadius[i] && BattleManager.GetInstance.IsCharacterThere(i))
            {
                SkillTriggerAreas[i].SetActive(true);
                
                
                //collider2d의 위치와 size를 BattleManager.GetInstance.GetCharacterFromIndex(i)꺼랑 같이 맞춤.
                var characterCollider = BattleManager.GetInstance.GetCharacterFromIndex(i).GetComponent<BoxCollider2D>();
                var skillTriggerCollider = SkillTriggerAreas[i].GetComponent<BoxCollider2D>();
                
                SkillTriggerAreas[i].transform.position =
                    BattleManager.GetInstance.GetCharacterFromIndex(i).transform.position;
                skillTriggerCollider.offset = characterCollider.offset;
                skillTriggerCollider.size = characterCollider.size;
                //debugrect도 같이 position과 size를 맞춰줌
                var skillTriggerDebugRect = SkillTriggerAreas[i].GetComponent<DebugRect>();
                skillTriggerDebugRect.Center = SkillTriggerAreas[i].transform.position;
                skillTriggerDebugRect.Size = skillTriggerCollider.size;
            }
        }
    }

    /// <summary>
    /// AbilityTriggerAreas 전부 비활성화시킨다.
    /// BattleManager의 Event 등록을 위해 의미없는 인자를 넣음
    /// </summary>
    void DeactivateSkillTriggerAreas(BaseCharacter character = null, bool isEnable = false)
    {
        foreach (var area in SkillTriggerAreas)
        {
            area.SetActive(false);
        }
    }
}
