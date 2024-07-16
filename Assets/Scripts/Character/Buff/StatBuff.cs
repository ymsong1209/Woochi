using TMPro;
using UnityEngine;

public class StatBuff : BaseBuff
{
    public string StatBuffName;

    #region 변화된 스탯들의 수치
    [SerializeField] protected Stat changeStat;
    #endregion 변화된 스탯들
    public StatBuff()
    {
        buffEffect = BuffEffect.StatStrengthen;
        buffType = BuffType.Positive;
    }
    
    public override void AddBuff(BaseCharacter _buffOwner)
    {
       base.AddBuff(_buffOwner);
       buffOwner.CheckForStatChange();
    }
    
    public override void StackBuff(BaseBuff _buff)
    {
        StatBuff statBuff = _buff as StatBuff;
        if (!statBuff) return;
        //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        if(_buff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        else base.buffDurationTurns += _buff.BuffDurationTurns;
        
        changeStat += statBuff.ChangeStat;
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
        if(BuffDurationTurns == -1)
        {
            description = StatBuffName + ": ";
        }
        else
        {
            description =  StatBuffName + BuffDurationTurns+": ";
        }
        if (changeStat.defense > 0)
        {
            description += "방어력 : +" + changeStat.defense + " ";
        }
        if (changeStat.crit > 0)
        {
            description += "치명타 : +" + changeStat.crit + " ";
        }
        if (changeStat.accuracy > 0)
        {
            description += "명중 : +" + changeStat.accuracy + " ";
        }
        if (changeStat.evasion > 0)
        {
            description += "회피 : +" + changeStat.evasion + " ";
        }
        if (changeStat.resist > 0)
        {
            description += "저항 : +" + changeStat.resist + " ";
        }
        if (changeStat.minStat > 0)
        {
            description += "최소 스탯 : +" + changeStat.minStat + " ";
        }
        if (changeStat.maxStat > 0)
        {
            description += "최대 스탯 : +" + changeStat.maxStat + " ";
        }
        if (changeStat.speed > 0)
        {
            description += "속도 : +" + changeStat.speed + " ";
        }
        description += "\n";
        text.text += description;
        text.color = Color.blue;
    }

    #region 변화된 스탯들의 수치 Getter Setter
    public Stat ChangeStat
    {
        get => changeStat;
        set => changeStat = value;
    }

    #endregion 변화된 스탯들의 수치 Getter Setter
}