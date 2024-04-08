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
            Button btn = skillIcons[i].GetComponent<Button>();
            btn.onClick.AddListener(() => SkillButtonClicked(skillIcons[index].Skill));
        }
    }

    // UI ������Ʈ�� �ʱ�ȭ�ϰ� ��ų ���� ��ư�� Ȱ��ȭ�ϴ� �޼���
    public void ShowForCharacter(BaseCharacter _character)
    {
        DisableSkills();

        #region ��ų ������ Enable, Disable ����
        int lastSkillIcon = skillIcons.Count - 1;
        int lastCharacterSkill = _character.activeSkills.Count - 1;

        // �� ĳ������ ��ų ������ŭ ��ư ������Ʈ Ȱ��ȭ
        for (int i = 0; i < lastSkillIcon; i++)
        {
            if(i < lastCharacterSkill)
            {
                skillIcons[i].gameObject.SetActive(true);

                // ��ų �����ܿ� ��ų ���� �Ҵ�
                skillIcons[i].SetSkill(_character.activeSkills[i]); 
            }
        }

        // ������ ��ų�� ��ġ �̵��̶� �����ϸ�, �������� ��ġ�ϰ� ����
        skillIcons[lastSkillIcon].SetSkill(_character.activeSkills[lastCharacterSkill]);
        #endregion

        // TODO : ��ų�� ��� ���� ����, ���� ���� ����� ��ư interaction ���� �����ؾ� ��
    }

    // ��ų ���� ��ư�� Ŭ������ �� ȣ��� �޼���
    public void SkillButtonClicked(BaseSkill _skill)
    {
        if (_skill == null || BattleManager.GetInstance.isSkillExecuted)
            return;

        // BattleManager�� SkillSelected �޼��� ȣ��
        onSkillSelected.Invoke(_skill);
    }

    /// <summary>
    /// ��� ��ų ������ interaction�� false�� �ʱ�ȭ
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
