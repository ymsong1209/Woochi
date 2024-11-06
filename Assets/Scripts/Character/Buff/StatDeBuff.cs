using TMPro;
using UnityEngine;

public class StatDeBuff : BaseBuff
{
    #region 변화된 스탯들의 수치
    public Stat changeStat;
    #endregion 변화된 스탯들
    public StatDeBuff()
    {
        changeStat = new Stat();
        buffEffect = BuffEffect.StatWeaken;
        buffType = BuffType.Negative;
    }
    public override void AddBuff(BaseCharacter caster, BaseCharacter _buffOwner)
    {
        base.AddBuff(caster, _buffOwner);
        buffOwner.CheckForStatChange();
    }
    
    public override int ApplyTurnStartBuff()
    {
        return 0;
    }
    public override int ApplyTurnEndBuff()
    {
        if(buffDurationTurns > 0) --buffDurationTurns;
        isBuffAppliedThisTurn = false;
        return 0;
    }
    
    public override void StackBuff(BaseBuff _buff)
    {
        StatDeBuff statDeBuff = _buff as StatDeBuff;
        if (!statDeBuff) return;
        //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        if(_buff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        else base.buffDurationTurns += _buff.BuffDurationTurns;
        base.buffBattleDurationTurns += _buff.BuffBattleDurationTurns;
        changeStat += statDeBuff.changeStat;
        buffOwner.CheckForStatChange();
    }

    public override void RemoveBuff()
    {
        buffOwner.CheckForStatChange();
        base.RemoveBuff();
    }

    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "";
        int statCount = 0;
    
        if (BuffDurationTurns == -1)
        {
            description = buffName + ": ";
        }
        else
        {
            description = buffName + " " + BuffDurationTurns + ": ";
        }
        
        for(int i = 1; i < (int)StatType.MaxDamage; i++)
        {
            if (!Mathf.Approximately(changeStat.GetValue((StatType)i), 0))
            {
                if (statCount > 0 && statCount % 2 == 0) description += "\n";
                description += ((StatType)i).GetDisplayName() + " : " + changeStat.GetValue((StatType)i) + " ";
                statCount++;
            }
        }
        // if (!Mathf.Approximately(changeStat.GetValue(StatType.Defense), 0))
        // {
        //     if (statCount > 0 && statCount % 2 == 0) description += "\n";
        //     description += "방어력 : " + changeStat.GetValue(StatType.Defense) + " ";
        //     statCount++;
        // }
        // if (!Mathf.Approximately(changeStat.GetValue(StatType.Crit), 0))
        // {
        //     if (statCount > 0 && statCount % 2 == 0) description += "\n";
        //     description += "치명타 : " + changeStat.GetValue(StatType.Crit) + " ";
        //     statCount++;
        // }
        // if (!Mathf.Approximately(changeStat.GetValue(StatType.Accuracy), 0))
        // {
        //     if (statCount > 0 && statCount % 2 == 0) description += "\n";
        //     description += "명중 : " + changeStat.GetValue(StatType.Accuracy) + " ";
        //     statCount++;
        // }
        // if (!Mathf.Approximately(changeStat.GetValue(StatType.Evasion), 0))
        // {
        //     if (statCount > 0 && statCount % 2 == 0) description += "\n";
        //     description += "회피 : " + changeStat.GetValue(StatType.Evasion) + " ";
        //     statCount++;
        // }
        // if (!Mathf.Approximately(changeStat.GetValue(StatType.Resist), 0))
        // {
        //     if (statCount > 0 && statCount % 2 == 0) description += "\n";
        //     description += "저항 : " + changeStat.GetValue(StatType.Resist) + " ";
        //     statCount++;
        // }
        // if (!Mathf.Approximately(changeStat.GetValue(StatType.MinDamage), 0))
        // {
        //     if (statCount > 0 && statCount % 2 == 0) description += "\n";
        //     description += "최소 스탯 : " + changeStat.GetValue(StatType.MinDamage) + " ";
        //     statCount++;
        // }
        // if (!Mathf.Approximately(changeStat.GetValue(StatType.MaxDamage), 0))
        // {
        //     if (statCount > 0 && statCount % 2 == 0) description += "\n";
        //     description += "최대 스탯 : " + changeStat.GetValue(StatType.MaxDamage) + " ";
        //     statCount++;
        // }
        // if (!Mathf.Approximately(changeStat.GetValue(StatType.Speed), 0))
        // {
        //     if (statCount > 0 && statCount % 2 == 0) description += "\n";
        //     description += "속도 : " + changeStat.GetValue(StatType.Speed) + " ";
        //     statCount++;
        // }
        description += "\n";
        text.text += description;
        SetBuffColor(text);
    }
}