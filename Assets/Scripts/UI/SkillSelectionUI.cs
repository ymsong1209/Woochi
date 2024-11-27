using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] // UnityEvent를 Inspector에서 볼 수 있게 하려면 Serializable이 필요합니다
public class SkillEvent : UnityEvent<BaseSkill> { }

public class SkillSelectionUI : MonoBehaviour
{
    //Inspector에서 등록
    public SkillEvent onSkillSelected; // SkillEvent 타입의 public 이벤트

    [SerializeField] protected SkillDescriptionUI skillDescriptionUI;
    [SerializeField] protected BuffDescriptionUI buffDescriptionUI;
    [SerializeField] private List<SkillIcon> skillIcons;
    private SkillIcon selectedIcon = null;

    private void Start()
    {
        #region 이벤트 등록
        BattleManager.GetInstance.ShowCharacterUI += ShowForCharacter;
        #endregion
        for (int i = 0; i < skillIcons.Count; i++)
        {
            int index = i;
            Button btn = skillIcons[i].btn;
            btn.onClick.AddListener(() => SkillButtonClicked(skillIcons[index]));

            skillIcons[i].OnShowTooltip += SetSkillTooltip;
            skillIcons[i].OnHideTooltip += () => skillDescriptionUI.gameObject.SetActive(false);
            skillIcons[i].OnHideTooltip += () => buffDescriptionUI.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 캐릭터에 맞는 스킬 아이콘 활성화시키는 메서드
    /// </summary>
    /// <param name="character"></param>
    /// <param name="isTurn">캐릭터 턴일때 호출됐는지</param>
    public void ShowForCharacter(BaseCharacter character, bool isTurn = true)
    {
        // 캐릭터가 전우치일 경우는 그냥 비활성화
        if (character.IsMainCharacter)
        {
            gameObject.SetActive(false);
            return;
        }
        else gameObject.SetActive(true);
        
        selectedIcon?.SetMark(false);
        
        if (character.IsAlly)
        { 
            character.CheckSkillsOnTurnStart();
            SetIcon(character, isTurn);
        }
        else
        {
            skillIcons.ForEach(icon => icon.btn.interactable = false);
        }
    }

    void SetIcon(BaseCharacter character, bool isTurn)
    {
        DisableSkills();
        
        int activeSkillsCount = character.activeSkills.Count;

        // 각 캐릭터의 스킬 개수만큼 버튼 오브젝트 활성화
        for (int i = 0; i < skillIcons.Count; i++)
        {
            if(i < activeSkillsCount)
            { 
                BaseSkill skill = character.activeSkills[i];
                // 스킬 아이콘에 스킬 정보 할당
                skillIcons[i].SetSkill(skill, (isTurn && character.IsSkillAvailable(skill)));
            }
        }
    }
    
    // 스킬 선택 버튼이 클릭됐을 때 호출될 메서드
    public void SkillButtonClicked(SkillIcon _skillIcon)
    {
        // 이전에 선택한 스킬 아이콘이 있다면 그 아이콘 선택 해제
        selectedIcon?.SetMark(false);
        selectedIcon = _skillIcon;
        selectedIcon.SetMark(true);
        
        // BattleManager의 SkillSelected 호출
        // SkillTriggerSelector의 Activate 메서드 호출
        onSkillSelected.Invoke(selectedIcon.Skill);
        AkSoundEngine.PostEvent("Movement_Click", gameObject);
    }

    /// <summary>
    /// 모든 스킬 아이콘 interaction을 false로 초기화
    /// </summary>
    private void DisableSkills()
    {
        selectedIcon?.SetMark(false);

        selectedIcon = null;

        foreach (SkillIcon icon in skillIcons)
        {
            icon.SetSkill(null);
        }
    }
    
    public void SetSkillTooltip(BaseSkill skill, Transform tr)
    {
        skillDescriptionUI.Activate(skill);
        RectTransform rt = tr as RectTransform;
        RectTransform targetRt = skillDescriptionUI.transform as RectTransform;
        targetRt.position = rt.position + new Vector3(0, rt.rect.height * 2f, 0);
        buffDescriptionUI.Activate(skill);
        buffDescriptionUI.SkillDescriptionUI = skillDescriptionUI;
        
    }
}
