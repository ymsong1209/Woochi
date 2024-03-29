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

    [SerializeField] private List<Button> skillList;

    private void Start()
    {
        #region 이벤트 등록
        BattleManager.GetInstance.OnCharacterTurnStart += ShowForCharacter;
        #endregion
        for (int i = 0; i < skillList.Count; i++)
        {
            int index = i;
            skillList[i].gameObject.SetActive(false);
            skillList[i].onClick.AddListener(() => SkillButtonClicked(skillList[index].GetComponent<BaseSkill>()));
        }
    }

    // UI 컴포넌트를 초기화하고 스킬 선택 버튼을 활성화하는 메서드
    public void ShowForCharacter(BaseCharacter _character)
    {
        // 각 캐릭터의 스킬 개수만큼 버튼 오브젝트 활성화
        for(int i = 0; i < skillList.Count; i++)
        {
            if(i < _character.skills.Count)
            {
                skillList[i].gameObject.SetActive(true);

                BaseSkill skill = skillList[i].gameObject.GetComponent<BaseSkill>();
                skill.Initialize(_character.skills[i], _character);
            }
            else
            {
                skillList[i].gameObject.SetActive(false);
            }
        }    
        
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


}
