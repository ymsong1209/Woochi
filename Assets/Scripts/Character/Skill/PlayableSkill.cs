using UnityEngine;

public class PlayableSkill : BaseSkill
{

    [SerializeField, ReadOnly] PlayableCharacter character;


    /// <summary>
    /// ��ų �����ܿ� ��ų�� ���콺�� ����, Ȥ�� Ű������ 1234��ư���� ��ų�� ���� �� ��� ��ų�� Ȱ��ȭ��Ű�� �Լ�
    /// ��ų�� Ȱ��ȭ�� �Ǹ� ��ų�� ������ ǥ�õȴ�.
    /// </summary>
    public override void SetSelect(bool _selected)
    {
        //��ų�� ������ ���
        if (_selected)
        {
            //������ Ȱ��ȭ�� ��ų�� ���� ���, �ش� ��ų�� ��Ȱ��ȭ�Ǿ����.
            if (character.IsSkillSelected)
            {

            }
            // ���� 
        }
        //��ų ���� ����
        else
        {

        }

        character.IsSkillSelected = _selected;

    }

    /// <summary>
    /// 
    /// </summary>
    public override void ActivateSkill()
    {
        //��ų �׵θ� icon�� ��������

        //skillSO

    }

    #region Validation
    private void OnValidate()
    {

    }
    #endregion
}