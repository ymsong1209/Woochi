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

    /// <summary>
    /// ĳ���Ϳ� �´� ��ų ������ Ȱ��ȭ��Ű�� �޼���
    /// </summary>
    /// <param name="_character"></param>
    /// <param name="isEnable">��ų Ȱ��ȭ ��ų�� ����</param>
    public void ShowForCharacter(BaseCharacter _character, bool isEnable = true)
    {
        // ���� �� ĳ���Ͷ�� skillIcon Interactable�� false�� �ʱ�ȭ
        if (!_character.IsAlly)
        {
            skillIcons.ForEach(icon => icon.btn.interactable = false);
            return;
        }

        DisableSkills();

        #region ��ų ������ Enable, Disable ����
        int activeSkills = _character.activeSkills.Count;

        // �� ĳ������ ��ų ������ŭ ��ư ������Ʈ Ȱ��ȭ
        for (int i = 0; i < skillIcons.Count; i++)
        {
            if(i < activeSkills)
            { 
                BaseSkill skill = _character.activeSkills[i];
                // ��ų �����ܿ� ��ų ���� �Ҵ�
                skillIcons[i].SetSkill(skill, (isEnable && IsSkillSetAvailable(skill)));
            }
        }
        
        #endregion
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
    /// ��� ��ų ������ interaction�� false�� �ʱ�ȭ
    /// </summary>
    private void DisableSkills()
    {
        foreach (SkillIcon icon in skillIcons)
        {
            icon.SetSkill(null);
        }
    }

}
