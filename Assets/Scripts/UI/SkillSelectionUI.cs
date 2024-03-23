using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] // UnityEvent�� Inspector���� �� �� �ְ� �Ϸ��� Serializable�� �ʿ��մϴ�
public class SkillEvent : UnityEvent<BaseSkill> { }

public class SkillSelectionUI : MonoBehaviour
{
    public SkillEvent onSkillSelected; // SkillEvent Ÿ���� public �̺�Ʈ

    // UI ������Ʈ�� �ʱ�ȭ�ϰ� ��ų ���� ��ư�� Ȱ��ȭ�ϴ� �޼���
    public void ShowForCharacter(BaseCharacter character)
    {
        // TODO: character�� ������ ��ų�� �������� UI ��ư ���� �� Ȱ��ȭ
        // ����: �� ��ų�� ���� ��ư�� �����, ��ư�� onClick �̺�Ʈ�� SkillButtonClicked�� ����

        // UI�� ǥ��
        gameObject.SetActive(true);
    }

    // ��ų ���� ��ư�� Ŭ������ �� ȣ��� �޼���
    public void SkillButtonClicked(BaseSkill skill)
    {
        // ���õ� ��ų�� �����ڿ��� �˸�
        onSkillSelected.Invoke(skill);

        // ��ų ���� UI�� ��Ȱ��ȭ
        Hide();
    }

    // ��ų ���� UI�� ����� �޼���
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
