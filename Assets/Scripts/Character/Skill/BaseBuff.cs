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

    /// <summary>
    /// 버프를 추가
    /// </summary>
    public virtual void AddBuff(BaseCharacter _buffOwner)
    {
        buffOwner = _buffOwner;
    }

    /// <summary>
    /// 전투가 시작될때 적용되는 버프 효과
    /// 사용 후 캐릭터가 살아있으면 true 반환
    /// <returns></returns>
    public virtual bool ApplyBattleStartBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// 라운드가 시작될때 적용되는 버프
    /// 버프 효과 사용 후 캐릭터가 살아있으면 true반환
    /// </summary>
    public virtual bool ApplyRoundStartBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// 자신의 차례가 됐을때 적용
    /// 버프 효과 사용 후 캐릭터가 살아있으면 true반환
    /// </summary>
    public virtual bool ApplyTurnStartBuff()
    {
        --buffDurationTurns;
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// 라운드가 끝날때 적용되는 버프
    /// 버프 효과 사용 후 캐릭터가 살아있으면 true반환
    /// </summary>
    public virtual bool ApplyRoundEndBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// 전투가 끝난 후 적용되는 버프 효과, default로 버프가 제거되게 함
    /// 사용 후 캐릭터 살아있으면 true 반환
    /// </summary>
    public virtual bool ApplyBattleEndBuff()
    {
        RemoveBuff();
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// 버프 제거
    /// 버프 제거 후 캐릭터가 살아있으면 true반환
    /// </summary>
    public virtual bool RemoveBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
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

