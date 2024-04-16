using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] // UnityEvent�� Inspector���� �� �� �ְ� �Ϸ��� Serializable�� �ʿ��մϴ�
public class SkillEvent : UnityEvent<BaseSkill> { }

public class SkillSelectionUI : MonoBehaviour
{
    public SkillEvent onSkillSelected; // SkillEvent Ÿ���� public �̺�Ʈ

    [SerializeField] private List<SkillIcon> skillIcons;
    [SerializeField] private Button turnOverButton;

    private void Start()
    {
        #region �̺�Ʈ ���
        BattleManager.GetInstance.OnCharacterTurnStart += ShowForCharacter;
        #endregion
        for (int i = 0; i < skillIcons.Count; i++)
        {
            int index = i;
            Button btn = skillIcons[i].btn;
            btn.onClick.AddListener(() => SkillButtonClicked(skillIcons[index].Skill));
        }
    }

    // UI ������Ʈ�� �ʱ�ȭ�ϰ� ��ų ���� ��ư�� Ȱ��ȭ�ϴ� �޼���
    public void ShowForCharacter(BaseCharacter _character)
    {
        DisableSkills();

        // ���� �� ĳ���Ͷ�� �׳� �ѱ��
        if (!_character.IsAlly)
            return;

        #region ��ų ������ Enable, Disable ����
        int lastSkillIcon = skillIcons.Count - 1;
        int lastCharacterSkill = _character.activeSkills.Count - 1;

        // �� ĳ������ ��ų ������ŭ ��ư ������Ʈ Ȱ��ȭ
        for (int i = 0; i < lastSkillIcon; i++)
        {
            if(i < lastCharacterSkill && IsSkillSetAvailable(_character.activeSkills[i]))
            { 
                skillIcons[i].gameObject.SetActive(true);

                // ��ų �����ܿ� ��ų ���� �Ҵ�
                skillIcons[i].SetSkill(_character.activeSkills[i]);
            }
        }

        // ������ ��ų�� ��ġ �̵��̶� �����ϸ�, �������� ��ġ�ϰ� ����
        skillIcons[lastSkillIcon].SetSkill(_character.activeSkills[lastCharacterSkill]);
        
        turnOverButton.interactable = true;
        #endregion

        // TODO : ��ų�� ��� ���� ����, ���� ���� ����� ��ư interaction ���� �����ؾ� ��
    }

    /// <summary>
    /// ��ų ���Կ� ��ų ��ư�� Ȱ��ȭ�� �Ǵ��� Ȯ��
    /// </summary>
    bool IsSkillSetAvailable(BaseSkill _skill)
    {
        if (IsSkillAbleForFormation(_skill) == false)   return false;
        if (IsSkillReceiverAble(_skill) == false)   return false;
        return true;
    }
    /// <summary>
    /// ���� ��ų�� owner�� ��ų�� ������ �� �ִ� ���� �ִ��� Ȯ��
    /// </summary>
    bool IsSkillAbleForFormation(BaseSkill _skill)
    {
        bool isSkillSetAble;

        int skillOwnerIndex = BattleManager.GetInstance.GetCharacterIndex(_skill.SkillOwner);
        isSkillSetAble = _skill.IsSkillAvailable(skillOwnerIndex);
        return isSkillSetAble;
    }

    /// <summary>
    /// ��ų�� ���� ����� �����ϴ��� Ȯ��
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

    // ��ų ���� ��ư�� Ŭ������ �� ȣ��� �޼���
    public void SkillButtonClicked(BaseSkill _skill)
    {
        if (_skill == null)
            return;

        // BattleManager�� SkillSelected ȣ��
        // SkillTriggerSelector�� Activate �޼��� ȣ��
        onSkillSelected.Invoke(_skill);
    }

    /// <summary>
    /// �� �ѱ�� ��ư Ŭ�� �� ȣ��� �޼���, BattleManager�� TurnOver �޼��� ȣ��
    /// </summary>
    public void TurnOverClicked() => BattleManager.GetInstance.TurnOver();

    /// <summary>
    /// ��� ��ų ������ interaction�� false�� �ʱ�ȭ
    /// </summary>
    private void DisableSkills()
    {
        foreach (SkillIcon icon in skillIcons)
        {
            icon.SetSkill(null);
        }

        turnOverButton.interactable = false;
    }

}
