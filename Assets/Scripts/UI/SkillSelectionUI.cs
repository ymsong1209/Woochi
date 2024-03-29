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

    [SerializeField] private List<Button> skillList;

    private void Start()
    {
        #region �̺�Ʈ ���
        BattleManager.GetInstance.OnCharacterTurnStart += ShowForCharacter;
        #endregion
        for (int i = 0; i < skillList.Count; i++)
        {
            int index = i;
            skillList[i].gameObject.SetActive(false);
            skillList[i].onClick.AddListener(() => SkillButtonClicked(skillList[index].GetComponent<BaseSkill>()));
        }
    }

    // UI ������Ʈ�� �ʱ�ȭ�ϰ� ��ų ���� ��ư�� Ȱ��ȭ�ϴ� �޼���
    public void ShowForCharacter(BaseCharacter _character)
    {
        // �� ĳ������ ��ų ������ŭ ��ư ������Ʈ Ȱ��ȭ
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


}
