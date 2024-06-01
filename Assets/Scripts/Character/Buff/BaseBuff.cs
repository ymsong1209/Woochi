using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;


public class BaseBuff : MonoBehaviour
{

    [SerializeField] protected BaseCharacter buffOwner;
    /// <summary>
    /// 현재 버프가 몇턴 남았는지
    /// buffDurationTurns가 -1이면 영구지속 버프
    /// </summary>
    [SerializeField] protected int  buffDurationTurns;
    [SerializeField] protected BuffType buffType;
    [SerializeField] protected int chanceToApplyBuff;

    #region 변화된 스탯들의 수치
    [SerializeField, ReadOnly] protected float changeDefense;
    [SerializeField, ReadOnly] protected float changeCrit;
    [SerializeField, ReadOnly] protected float changeAccuracy;
    [SerializeField, ReadOnly] protected float changeEvasion;
    [SerializeField, ReadOnly] protected float changeResist;
    [SerializeField, ReadOnly] protected float changeMinStat;
    [SerializeField, ReadOnly] protected float changeMaxStat;
    [SerializeField, ReadOnly] protected float changeSpeed;
    #endregion 변화된 스탯들

    /// <summary>
    /// 버프를 추가
    /// </summary>
    public virtual void AddBuff(BaseCharacter _buffOwner)
    {
        buffOwner = _buffOwner;
        buffOwner.activeBuffs.Add(this);
    }

    /// <summary>
    /// 전투가 시작될때 적용되는 버프 효과
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// <returns></returns>
    public virtual int ApplyBattleStartBuff()
    {
        if (buffOwner.CheckDead()) return -1;
        return 0;
    }

    /// <summary>
    /// 라운드가 시작될때 적용되는 버프
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyRoundStartBuff()
    {
        if (buffOwner.CheckDead()) return -1;
        return 0;
    }

    /// <summary>
    /// 자신의 차례가 됐을때 적용
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyTurnStartBuff()
    {
        if(buffDurationTurns > 0) --buffDurationTurns;
        if (buffOwner.CheckDead()) return -1;
        return 0;
    }
    /// <summary>
    /// 자신의 턴이 끝났을때 적용
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyTurnEndBuff()
    {
        if (buffOwner.CheckDead()) return -1;
        return 0;
    }

    /// <summary>
    /// 라운드가 끝날때 적용되는 버프
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyRoundEndBuff()
    {
        if (buffOwner.CheckDead()) return -1;
        return 0;
    }

    /// <summary>
    /// 전투가 끝난 후 적용되는 버프 효과, default로 버프가 제거되게 함
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyBattleEndBuff()
    {
        RemoveBuff();
        if (buffOwner.CheckDead()) return -1;
        return 0;
    }

    /// <summary>
    /// 버프 제거
    /// </summary>
    public virtual void RemoveBuff()
    {
        //현재 parent에 가서 bufficon을 가져옴
        BuffIcon icon = transform.parent.GetComponent<BuffIcon>();
        if (icon)
        {
            icon.CheckChildBuffs(this);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 중복해서 버프류를 쌓으려고 하는 경우
    /// </summary>
    public virtual void StackBuff(BaseBuff _buff)
    {
        buffDurationTurns += _buff.buffDurationTurns;
    }

    public virtual void SetBuffDescription(TextMeshProUGUI text)
    {
        text.text = "Buff Description\n";
        text.color = Color.magenta;
    }
    

    #region Getter Setter
    public int BuffDurationTurns
    {
        get { return buffDurationTurns; }
        set { buffDurationTurns = value; }
    }
    public BuffType BuffType
    {
        get => buffType;
        set => buffType = value;
    }
    
    public int ChanceToApplyBuff
    {
        get { return chanceToApplyBuff; }
        set { chanceToApplyBuff = value; }
    }
    
    #region 변화된 스탯들의 수치 Getter Setter
    public float ChangeDefense 
    {
        get { return changeDefense; }
        set { changeDefense = value; }
    }
    public float ChangeCrit 
    {
        get { return changeCrit; }
        set { changeCrit = value; }
    }
    public float ChangeAccuracy 
    {
        get { return changeAccuracy; }
        set { changeAccuracy = value; }
    }
    public float ChangeEvasion 
    {
        get { return changeEvasion; }
        set { changeEvasion = value; }
    }
    public float ChangeResist 
    {
        get { return changeResist; }
        set { changeResist = value; }
    }
    public float ChangeMinStat 
    {
        get { return changeMinStat; }
        set { changeMinStat = value; }
    }
    public float ChangeMaxStat 
    {
        get { return changeMaxStat; }
        set { changeMaxStat = value; }
    }
    public float ChangeSpeed 
    {
        get { return changeSpeed; }
        set { changeSpeed = value; }
    }
    #endregion 변화된 스탯들의 수치 Getter Setter

    
    #endregion Getter Setter

    #region Validation
    private void OnValidate()
    {
        // if(buffDurationTurns == 0)
        // {
        //     Debug.Log(nameof(buffDurationTurns) + " is not over 1 in object " + this.name.ToString());
        // }
        
    }
    #endregion
}

