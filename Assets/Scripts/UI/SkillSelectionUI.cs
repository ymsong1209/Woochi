using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] // UnityEvent를 Inspector에서 볼 수 있게 하려면 Serializable이 필요합니다
public class SkillEvent : UnityEvent<BaseSkill> { }

public class SkillSelectionUI : MonoBehaviour
{
    public SkillEvent onSkillSelected; // SkillEvent 타입의 public 이벤트

    [SerializeField] private List<SkillIcon> skillIcons;

    private void Start()
    {
        #region 이벤트 등록
        BattleManager.GetInstance.OnCharacterTurnStart += ShowForCharacter;
        #endregion
        for (int i = 0; i < skillIcons.Count; i++)
        {
            int index = i;
            Button btn = skillIcons[i].GetComponent<Button>();
            btn.onClick.AddListener(() => SkillButtonClicked(skillIcons[index].Skill));
        }
    }

    // UI 컴포넌트를 초기화하고 스킬 선택 버튼을 활성화하는 메서드
    public void ShowForCharacter(BaseCharacter _character)
    {
        DisableSkills();

        #region 스킬 아이콘 Enable, Disable 설정
        int lastSkillIcon = skillIcons.Count - 1;
        int lastCharacterSkill = _character.activeSkills.Count - 1;

        // 각 캐릭터의 스킬 개수만큼 버튼 오브젝트 활성화
        for (int i = 0; i < lastSkillIcon; i++)
        {
            if(i < lastCharacterSkill)
            {
                skillIcons[i].gameObject.SetActive(true);

                // 스킬 아이콘에 스킬 정보 할당
                skillIcons[i].SetSkill(_character.activeSkills[i]); 
            }
        }

        // 마지막 스킬을 위치 이동이라 가정하면, 마지막에 위치하게 했음
        skillIcons[lastSkillIcon].SetSkill(_character.activeSkills[lastCharacterSkill]);
        #endregion

        // TODO : 스킬의 사용 가능 여부, 범위 등을 고려해 버튼 interaction 여부 결정해야 함
    }

    // 스킬 선택 버튼이 클릭됐을 때 호출될 메서드
    public void SkillButtonClicked(BaseSkill _skill)
    {
        if (_skill == null || BattleManager.GetInstance.isSkillExecuted)
            return;

        // BattleManager의 SkillSelected 메서드 호출
        onSkillSelected.Invoke(_skill);
    }

    /// <summary>
    /// 모든 스킬 아이콘 interaction을 false로 초기화
    /// </summary>
    private void DisableSkills()
    {
        skillIcons.ForEach(icon => icon.GetComponent<Button>().interactable = false);
        foreach (SkillIcon icon in skillIcons)
        {
            icon.SetSkill(null);
        }
    }

}
