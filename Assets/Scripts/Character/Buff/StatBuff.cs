using TMPro;
using UnityEngine;

public class StatBuff : BaseBuff
{
    public Stat changeStat;

    public StatBuff()
    {
        changeStat = new Stat();
        buffEffect = BuffEffect.StatStrengthen;
        buffType = BuffType.Positive;
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
        StatBuff statBuff = _buff as StatBuff;
        if (!statBuff) return;
        Logger.BattleLog($"\"{buffOwner.Name}({buffOwner.RowOrder + 1})\"에게 \"{buffName}\" 버프가 중첩되었습니다.", "버프 중첩");
        
        //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        if(_buff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        else base.buffDurationTurns += _buff.BuffDurationTurns;
        base.buffBattleDurationTurns += _buff.BuffBattleDurationTurns;
        changeStat += statBuff.changeStat;
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