using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseBuff : MonoBehaviour
{

    [SerializeField] protected BaseCharacter buffOwner;
    /// <summary>
    /// buffDurationTurns가 -1이면 영구지속 버프
    /// </summary>
    [SerializeField] protected int buffDurationTurns;
    [SerializeField] protected BuffType buffType;

    public virtual void AddBuff(BaseCharacter _buffOwner)
    {
        buffOwner = _buffOwner;
    }

    /// <summary>
    /// 라운드가 시작될때 적용되는 버프
    /// </summary>
    public virtual void ApplyRoundStartBuff()
    {

    }

    /// <summary>
    /// 자신의 차례가 됐을때 적용
    /// </summary>
    public virtual void ApplyBuffWhenMyTurn()
    {
        --buffDurationTurns;
    }

    /// <summary>
    /// 라운드가 끝날때 적용되는 버프
    /// </summary>
    public virtual void ApplyRoundEndBuff()
    {

    }

    public virtual void RemoveBuff()
    {
        if(buffOwner == null)
        {
            Debug.LogError("No BuffOwner"); return;
        }


    }

    /// <summary>
    /// _bufftype이 나에게 적용될 수 있는지 확인
    /// </summary>
    public virtual bool ValidateApplyBuff(BuffType _bufftype)
    {
        return true;
    }

    #region Getter Setter
    public int BuffDurationTurns
    {
        get { return buffDurationTurns; }
        set { buffDurationTurns = value; }
    }
    public BuffType BuffType
    {
        get { return buffType; }
        set { buffType = value; }
    }
    #endregion Getter Setter

    #region Validation
    private void OnValidate()
    {
        if(buffDurationTurns == 0)
        {
            Debug.Log(nameof(buffDurationTurns) + " is not over 1 in object " + this.name.ToString());
        }
        
    }
    #endregion
}

