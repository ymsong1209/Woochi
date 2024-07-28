using TMPro;
using UnityEngine;

public class StatDeBuff : BaseBuff
{
    #region 변화된 스탯들의 수치
    [SerializeField] protected Stat changeStat;
    #endregion 변화된 스탯들
    public StatDeBuff()
    {
        buffEffect = BuffEffect.StatStrengthen;
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
        return 0;
    }
    
    public override void StackBuff(BaseBuff _buff)
    {
        StatDeBuff statDeBuff = _buff as StatDeBuff;
        if (!statDeBuff) return;
        //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        if(_buff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        else base.buffDurationTurns += _buff.BuffDurationTurns;
        
        changeStat += statDeBuff.ChangeStat;
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

        if (changeStat.defense < 0)
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "방어력 : " + changeStat.defense + " ";
            statCount++;
        }
        if (changeStat.crit < 0)
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "치명타 : " + changeStat.crit + " ";
            statCount++;
        }
        if (changeStat.accuracy < 0)
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "명중 : " + changeStat.accuracy + " ";
            statCount++;
        }
        if (changeStat.evasion < 0)
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "회피 : " + changeStat.evasion + " ";
            statCount++;
        }
        if (changeStat.resist < 0)
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "저항 : " + changeStat.resist + " ";
            statCount++;
        }
        if (changeStat.minStat < 0)
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "최소 스탯 : " + changeStat.minStat + " ";
            statCount++;
        }
        if (changeStat.maxStat < 0)
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "최대 스탯 : " + changeStat.maxStat + " ";
            statCount++;
        }
        if (changeStat.speed < 0)
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "속도 : " + changeStat.speed + " ";
            statCount++;
        }
        description += "\n";
        text.text += description;
        SetBuffColor(text);
    }


    #region 변화된 스탯들의 수치 Getter Setter
    public Stat ChangeStat
    {
        get => changeStat;
        set => changeStat = value;
    }

    #endregion 변화된 스탯들의 수치 Getter Setter
}