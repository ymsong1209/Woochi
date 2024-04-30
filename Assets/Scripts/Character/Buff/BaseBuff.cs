using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseBuff : MonoBehaviour
{

    [SerializeField] protected BaseCharacter buffOwner;
    /// <summary>
    /// ������ �ʱ� ���ӽð�
    /// </summary>
    [SerializeField] protected int baseBuffDurationTurns;
    /// <summary>
    /// ���� ������ ���� ���Ҵ���
    /// buffDurationTurns�� -1�̸� �������� ����
    /// </summary>
    [SerializeField] protected int  buffDurationTurns;
    [SerializeField] protected BuffType buffType;
    [SerializeField] protected int chanceToApplyBuff;

    #region ��ȭ�� ���ȵ��� ��ġ
    [SerializeField, ReadOnly] protected float changeDefense;
    [SerializeField, ReadOnly] protected float changeCrit;
    [SerializeField, ReadOnly] protected float changeAccuracy;
    [SerializeField, ReadOnly] protected float changeEvasion;
    [SerializeField, ReadOnly] protected float changeResist;
    [SerializeField, ReadOnly] protected float changeMinStat;
    [SerializeField, ReadOnly] protected float changeMaxStat;
    #endregion ��ȭ�� ���ȵ�

    #region ���ҽ�Ű�� ���� Stat��
    [SerializeField, ReadOnly] protected float leftoverDefense;
    [SerializeField, ReadOnly] protected float leftoverCrit;
    [SerializeField, ReadOnly] protected float leftoverAccuracy;
    [SerializeField, ReadOnly] protected float leftoverEvasion;
    [SerializeField, ReadOnly] protected float leftoverResist;
    [SerializeField, ReadOnly] protected float leftoverMinStat;
    [SerializeField, ReadOnly] protected float leftoverMaxStat;
    #endregion

    /// <summary>
    /// ������ �߰�
    /// </summary>
    public virtual void AddBuff(BaseCharacter _buffOwner)
    {
        buffOwner = _buffOwner;
        buffOwner.activeBuffs.Add(this);
        buffDurationTurns = baseBuffDurationTurns;
    }

    /// <summary>
    /// ������ ���۵ɶ� ����Ǵ� ���� ȿ��
    /// ��� �� ĳ���Ͱ� ��������� true ��ȯ
    /// <returns></returns>
    public virtual bool ApplyBattleStartBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// ���尡 ���۵ɶ� ����Ǵ� ����
    /// ���� ȿ�� ��� �� ĳ���Ͱ� ��������� true��ȯ
    /// </summary>
    public virtual bool ApplyRoundStartBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// �ڽ��� ���ʰ� ������ ����
    /// ���� ȿ�� ��� �� ĳ���Ͱ� ��������� true��ȯ
    /// </summary>
    public virtual bool ApplyTurnStartBuff()
    {
        --buffDurationTurns;
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// ���尡 ������ ����Ǵ� ����
    /// ���� ȿ�� ��� �� ĳ���Ͱ� ��������� true��ȯ
    /// </summary>
    public virtual bool ApplyRoundEndBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// ������ ���� �� ����Ǵ� ���� ȿ��, default�� ������ ���ŵǰ� ��
    /// ��� �� ĳ���� ��������� true ��ȯ
    /// </summary>
    public virtual bool ApplyBattleEndBuff()
    {
        RemoveBuff();
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// ���� ����
    /// ���� ���� �� ĳ���Ͱ� ��������� true��ȯ
    /// </summary>
    public virtual bool RemoveBuff()
    {
        //������ �����ϸ鼭 leftoverstat��ŭ basecharacter�� stat�� ���ҽ�Ŵ
        //������ġ�� RemoveBuff�� Override�� �Լ����� ������.
        DecreaseStatFromLeftOverStat();
        
        //���� ������ �����ϸ鼭 �ٸ� ������ ��ġ ������ �϶�� ��
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
    /// �ߺ��ؼ� �������� �������� �ϴ� ���
    /// </summary>
    public virtual void StackBuff()
    {
        buffDurationTurns += baseBuffDurationTurns;
    }

    /// <summary>
    /// ������ �����ɶ� LeftoverStat��ŭ�� ������ ���� ���ҽ�Ŵ
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

    //�ٸ� ������ ���� �� ��쿡 �ߵ�, ���� ������ ���ҽ�Ű�� ���� stat�� ������ ���� ���ҽ�Ŵ
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
        // ReduceStatWithClamp�� ȣ���Ͽ� ���� ���ҷ��� ����Ѵ�.
        float newStatValue = ReduceStatWithClamp(currentStatValue, ref _reductionAmount);

        // ���� ���ο� ���� ���� ��ȯ.
        return newStatValue;
    }

    private float ReduceStatWithClamp(float currentValue, ref float _reductionAmount)
    {
        float originalValue = currentValue; // ���� ���� �� ����
        currentValue -= _reductionAmount;
        currentValue = Mathf.Clamp(currentValue, 0, float.MaxValue); // 0 ���Ϸ� �������� �ʰ� ����

        if (currentValue <= 0)
        {
            // ���� ���ҽ�ų �� ������ ���� ����Ͽ� reductionAmount�� ����
            _reductionAmount = _reductionAmount - originalValue;
        }
        else
        {
            // ������ ���������� ���ҵǾ��ٸ�, ���� ���ҷ��� �����Ƿ� reductionAmount�� 0���� ����
            _reductionAmount = 0;
        }

        return currentValue; // ������ ���� �� ��ȯ
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
        if(buffDurationTurns == 0)
        {
            Debug.Log(nameof(buffDurationTurns) + " is not over 1 in object " + this.name.ToString());
        }
        
    }
    #endregion
}

