using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayableCharacter : BaseCharacter
{
    #region Header Private 변수들
    #region Battle 관련된 변수
    [Tooltip("BattleManager에서 관리, 전투중인지 확인")]
    [SerializeField, ReadOnly] private bool isBattleActive;   

    [Tooltip("BaseSkill에서 관리, 스킬이 활성화되었는지 확인")]
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