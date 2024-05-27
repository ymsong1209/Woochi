using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] // UnityEvent를 Inspector에서 볼 수 있게 하려면 Serializable이 필요합니다
public class SkillEvent : UnityEvent<BaseSkill> { }

public class SkillSelectionUI : MonoBehaviour
{
    //Inspector에서 등록
    public SkillEvent onSkillSelected; // SkillEvent 타입의 public 이벤트

    [SerializeField] private List<SkillIcon> skillIcons;

    private void Start()
    {
        #region 이벤트 등록
        BattleManager.GetInstance.OnCharacterTurnStart += ShowForCharacter;
        BattleManager.GetInstance.OnCharacterAttacked += ShowForCharacter;
        #endregion
        for (int i = 0; i < skillIcons.Count; i++)
        {
            int index = i;
            Button btn = skillIcons[i].btn;
            btn.onClick.AddListener(() => SkillButtonClicked(skillIcons[index].Skill));
        }
    }

    /// <summary>
    /// 캐릭터에 맞는 스킬 아이콘 활성화시키는 메서드
    /// </summary>
    /// <param name="_character"></param>
    /// <param name="isEnable">스킬 활성화 시킬지 말지</param>
    public void ShowForCharacter(BaseCharacter _character, bool isEnable = true)
    {
        // 현재 캐릭터의 턴이 전우치라면 UI 비활성화
        if (_character.IsMainCharacter) gameObject.SetActive(false);
        else gameObject.SetActive(true);
        
        _character.CheckSkillsOnTurnStart();

        // 턴이 적 캐릭터라면 skillIcon Interactable을 false로 초기화
        if (!_character.IsAlly)
        {
            skillIcons.ForEach(icon => icon.btn.interactable = false);
            return;
        }

        DisableSkills();

        #region 스킬 아이콘 Enable, Disable 설정
        int activeSkillsCount = _character.activeSkills.Count;

        // 각 캐릭터의 스킬 개수만큼 버튼 오브젝트 활성화
        for (int i = 0; i < skillIcons.Count; i++)
        {
            if(i < activeSkillsCount)
            { 
                BaseSkill skill = _character.activeSkills[i];
                // 스킬 아이콘에 스킬 정보 할당
                skillIcons[i].SetSkill(skill, (isEnable && IsSkillSetAvailable(skill)));
            }
        }
        
        #endregion
    }

    /// <summary>
    /// 스킬 슬롯에 스킬 버튼이 활성화가 되는지 확인
    /// </summary>
    bool IsSkillSetAvailable(BaseSkill _skill)
    {
        if (IsSkillAbleForFormation(_skill) == false)   return false;
        if (IsSkillReceiverAble(_skill) == false)   return false;
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

    // 스킬 선택 버튼이 클릭됐을 때 호출될 메서드
    public void SkillButtonClicked(BaseSkill _skill)
    {
        if (_skill == null)
            return;

        // BattleManager의 SkillSelected 호출
        // SkillTriggerSelector의 Activate 메서드 호출
        onSkillSelected.Invoke(_skill);
    }

    /// <summary>
    /// 모든 스킬 아이콘 interaction을 false로 초기화
    /// </summary>
    private void DisableSkills()
    {
        foreach (SkillIcon icon in skillIcons)
        {
            icon.SetSkill(null);
        }
    }
    
}
