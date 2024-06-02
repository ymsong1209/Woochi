using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class WoochiSkillSelectionUI : MonoBehaviour
{
    public SkillEvent onSkillSelected;
    [SerializeField] private List<SkillIcon> skillIcons;

    public void Start()
    {
        for (int i = 0; i < skillIcons.Count; i++)
        {
            int index = i;
            Button btn = skillIcons[i].btn;
            btn.onClick.AddListener(() => SkillButtonClicked(skillIcons[index].Skill));
        }
    }

    public void Initialize()
    {
        gameObject.SetActive(false);
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter)
        {
            //Debug.LogError("우치가 아님");
            return;
        }
        UIManager.GetInstance.SetSorceryPointUI(mainCharacter.SorceryPoints);
        UIManager.GetInstance.SetSorceryPointBackgroundUI(mainCharacter.SorceryPoints);
    }
    
    public void Activate()
    {
        gameObject.SetActive(true);
        
        BaseCharacter MainCharacter = BattleManager.GetInstance.currentCharacter;
        if (!MainCharacter.IsMainCharacter)
        {
            Debug.LogError("우치 차례가 아님");
            return;
        }
        
        //우치 위치에 따른 스킬 체크
        MainCharacter.CheckSkillsOnTurnStart();
        //모든 스킬 비활성화
        DisableSkills();

        #region 스킬 아이콘 Enable, Disable 설정
        int activeSkillsCount = MainCharacter.activeSkills.Count;
        if (activeSkillsCount > 4)
        {
            Debug.LogError("우치 스킬이 4개 이상이 활성화되어있음.");
            return;
        }
        
        // 각 캐릭터의 스킬 개수만큼 버튼 오브젝트 활성화
        for (int i = 0; i < skillIcons.Count; i++)
        {
            if(i < activeSkillsCount)
            { 
                BaseSkill skill = MainCharacter.activeSkills[i];
                // 스킬 아이콘에 스킬 정보 할당
                skillIcons[i].SetSkill(skill, (IsSkillSetAvailable(skill)));
            }
        }
        
        #endregion
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 스킬 슬롯에 스킬 버튼이 활성화가 되는지 확인
    /// </summary>
    bool IsSkillSetAvailable(BaseSkill skill)
    {
        MainCharacter mainCharacter = skill.SkillOwner as MainCharacter;
        MainCharacterSkill mainCharacterSkill = skill as MainCharacterSkill;
        if(mainCharacter == null || mainCharacterSkill == null) return false;
        if (!IsSkillAbleForFormation(mainCharacterSkill))   return false;
        if (!IsSkillReceiverAble(mainCharacterSkill))   return false;
        if(mainCharacter.SorceryPoints < mainCharacterSkill.RequiredSorceryPoints) return false;
        return true;
    }
    
    /// <summary>
    /// 현재 스킬의 owner가 스킬을 시전할 수 있는 열에 있는지 확인
    /// </summary>
    bool IsSkillAbleForFormation(BaseSkill _skill)
    {
        bool isSkillSetAble;

        int skillOwnerIndex = BattleManager.GetInstance.GetCharacterIndex(_skill.SkillOwner);
        isSkillSetAble = _skill.IsSkillAvailable(skillOwnerIndex);
        return isSkillSetAble;
    }

    /// <summary>
    /// 스킬의 적용 대상이 존재하는지 확인
    /// </summary>
    bool IsSkillReceiverAble(BaseSkill _skill)
    {
        for(int i = 0; i < _skill.SkillRadius.Length; ++i)
        {
            if (_skill.SkillRadius[i] && BattleManager.GetInstance.IsCharacterThere(i))
                return true;
        }

        return false;
    }
    
    private void DisableSkills()
    {
        foreach (SkillIcon icon in skillIcons)
        {
            icon.SetSkill(null);
        }
    }
    
    public void SkillButtonClicked(BaseSkill _skill)
    {
        if (_skill == null)
            return;

        // BattleManager의 SkillSelected 호출
        // SkillTriggerSelector의 Activate 메서드 호출
        onSkillSelected.Invoke(_skill);
    }
    
    public SkillEvent OnSkillSelected => onSkillSelected;
}
