﻿using TMPro;
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
        buffStackType = BuffStackType.ResetDuration;
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
        if (!statDeBuff || _buff.BuffName != BuffName) return;
        base.StackBuff(_buff);
        Logger.BattleLog($"\"{buffOwner.Name}({buffOwner.RowOrder + 1})\"에게 \"{buffName}\" 버프가 중첩되었습니다.", "버프 중첩");
        buffOwner.CheckForStatChange();
    }
    protected override void StackBuffEffect(BaseBuff _buff)
    {
        StatDeBuff statDeBuff = _buff as StatDeBuff;
        changeStat += statDeBuff.changeStat;
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
            description = buffName + " " + BuffDurationTurns + "턴: ";
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
        
        description += "\n";
        text.text += description;
        SetBuffColor(text);
    }
}