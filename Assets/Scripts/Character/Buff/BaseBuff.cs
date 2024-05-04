using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseBuff : MonoBehaviour
{

    [SerializeField] protected BaseCharacter buffOwner;
    /// <summary>
    /// 버프의 초기 지속시간
    /// </summary>
    [SerializeField] protected int baseBuffDurationTurns;
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
    #endregion 변화된 스탯들

    #region 감소시키지 못한 Stat들
    [SerializeField, ReadOnly] protected float leftoverDefense;
    [SerializeField, ReadOnly] protected float leftoverCrit;
    [SerializeField, ReadOnly] protected float leftoverAccuracy;
    [SerializeField, ReadOnly] protected float leftoverEvasion;
    [SerializeField, ReadOnly] protected float leftoverResist;
    [SerializeField, ReadOnly] protected float leftoverMinStat;
    [SerializeField, ReadOnly] protected float leftoverMaxStat;
    #endregion

    /// <summary>
    /// 버프를 추가
    /// </summary>
    public virtual void AddBuff(BaseCharacter _buffOwner)
    {
        buffOwner = _buffOwner;
        buffOwner.activeBuffs.Add(this);
        buffDurationTurns = baseBuffDurationTurns;
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
        //버프를 제거하면서 leftoverstat만큼 basecharacter의 stat를 감소시킴
        //증가수치는 RemoveBuff를 Override한 함수에서 결정함.
        DecreaseStatFromLeftOverStat();
        
        //먼저 버프를 제거하면서 다른 버프에 수치 조절을 하라고 함
        foreach(BaseBuff buff in buffOwner.activeBuffs)
        {
            if(buff != this)
            {
                DecreaseStatWithClampFromLeftOverStat();
            }
        }

        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// 중복해서 버프류를 쌓으려고 하는 경우
    /// </summary>
    public virtual void StackBuff()
    {
        buffDurationTurns += baseBuffDurationTurns;
    }

    /// <summary>
    /// 버프가 해제될때 LeftoverStat만큼의 스탯을 먼저 감소시킴
    /// </summary>
    public virtual void DecreaseStatFromLeftOverStat()
    {
        buffOwner.Defense -= leftoverDefense;
        buffOwner.Crit -= leftoverCrit;
        buffOwner.Accuracy -= leftoverAccuracy;
        buffOwner.Evasion -= leftoverEvasion;
        buffOwner.Resist -= leftoverResist;
        buffOwner.MinStat -= leftoverMinStat;
        buffOwner.MaxStat -= leftoverMaxStat;
    }

    //다른 버프가 해제 될 경우에 발동, 현재 버프가 감소시키지 못한 stat이 있으면 마저 감소시킴
    public virtual void DecreaseStatWithClampFromLeftOverStat()
    {
        buffOwner.Defense = ExecuteLeftOverStatReduction(buffOwner.Defense, ref leftoverDefense);
        buffOwner.Crit = ExecuteLeftOverStatReduction(buffOwner.Crit, ref leftoverCrit);
        buffOwner.Accuracy = ExecuteLeftOverStatReduction(buffOwner.Accuracy, ref leftoverAccuracy); 
        buffOwner.Evasion = ExecuteLeftOverStatReduction(buffOwner.Evasion, ref leftoverEvasion);
        buffOwner.Resist = ExecuteLeftOverStatReduction(buffOwner.Resist, ref leftoverResist);
        buffOwner.MinStat = ExecuteLeftOverStatReduction(buffOwner.MinStat, ref leftoverMinStat);
        buffOwner.MaxStat = ExecuteLeftOverStatReduction(buffOwner.MaxStat, ref leftoverMaxStat);
    }

    protected float ExecuteLeftOverStatReduction(float currentStatValue, ref float _reductionAmount)
    {
        // ReduceStatWithClamp를 호출하여 남은 감소량을 계산한다.
        float newStatValue = ReduceStatWithClamp(currentStatValue, ref _reductionAmount);

        // 계산된 새로운 스탯 값을 반환.
        return newStatValue;
    }

    private float ReduceStatWithClamp(float currentValue, ref float _reductionAmount)
    {
        float originalValue = currentValue; // 원래 스탯 값 저장
        currentValue -= _reductionAmount;
        currentValue = Mathf.Clamp(currentValue, 0, float.MaxValue); // 0 이하로 떨어지지 않게 조정

        if (currentValue <= 0)
        {
            // 실제 감소시킬 수 없었던 양을 계산하여 reductionAmount에 저장
            _reductionAmount = _reductionAmount - originalValue;
        }
        else
        {
            // 스탯이 성공적으로 감소되었다면, 남은 감소량이 없으므로 reductionAmount를 0으로 설정
            _reductionAmount = 0;
        }

        return currentValue; // 조정된 스탯 값 반환
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

    public int ChanceToApplyBuff => chanceToApplyBuff;
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

