using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] // UnityEvent를 Inspector에서 볼 수 있게 하려면 Serializable이 필요합니다
public class SkillEvent : UnityEvent<BaseSkill> { }

public class SkillSelectionUI : MonoBehaviour
{
    public SkillEvent onSkillSelected; // SkillEvent 타입의 public 이벤트

    // UI 컴포넌트를 초기화하고 스킬 선택 버튼을 활성화하는 메서드
    public void ShowForCharacter(BaseCharacter character)
    {
        // TODO: character의 가능한 스킬을 바탕으로 UI 버튼 생성 및 활성화
        // 예시: 각 스킬에 대해 버튼을 만들고, 버튼의 onClick 이벤트에 SkillButtonClicked를 연결

        // UI를 표시
        gameObject.SetActive(true);
    }

    // 스킬 선택 버튼이 클릭됐을 때 호출될 메서드
    public void SkillButtonClicked(BaseSkill skill)
    {
        // 선택된 스킬을 구독자에게 알림
        onSkillSelected.Invoke(skill);

        // 스킬 선택 UI를 비활성화
        Hide();
    }

    // 스킬 선택 UI를 숨기는 메서드
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
