using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayableCharacter : BaseCharacter
{
    #region Header Private ������
    #region Battle ���õ� ����
    [Tooltip("BattleManager���� ����, ���������� Ȯ��")]
    [SerializeField, ReadOnly] private bool isBattleActive;   

    [Tooltip("BaseSkill���� ����, ��ų�� Ȱ��ȭ�Ǿ����� Ȯ��")]
    [SerializeField, ReadOnly] private bool isSkillSelected;

    #endregion
    #endregion


    public override void Initialize(){
        base.Initialize();
        isSkillSelected = false;
    }

    public override void SetDead(bool _dead)
    {
        base.SetDead(_dead);
    }

    #region Getter, Setter

    public bool IsBattleActive
    {
        get { return isBattleActive; }
        set { isBattleActive = value; }
    }

    public bool IsSkillSelected
    {
        get { return isSkillSelected; }
        set { isSkillSelected = value; }
    }

    #endregion
}