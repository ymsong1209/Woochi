using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatBuff : BaseBuff
{
    public string StatBuffName;
    
    #region 변화된 스탯들의 수치
    [SerializeField] protected float changeDefense;
    [SerializeField] protected float changeCrit;
    [SerializeField] protected float changeAccuracy;
    [SerializeField] protected float changeEvasion;
    [SerializeField] protected float changeResist;
    [SerializeField] protected float changeMinStat;
    [SerializeField] protected float changeMaxStat;
    [SerializeField] protected float changeSpeed;
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
        
        changeAccuracy += statBuff.ChangeAccuracy;
        changeSpeed += statBuff.ChangeSpeed;
        changeDefense += statBuff.ChangeDefense;
        changeCrit += statBuff.ChangeCrit;
        changeEvasion += statBuff.ChangeEvasion;
        changeResist += statBuff.ChangeResist;
        changeMinStat += statBuff.ChangeMinStat;
        changeMaxStat += statBuff.ChangeMaxStat;
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
        if (changeDefense > 0)
        {
            description += "방어력 : +" + changeDefense + " ";
        }
        if (changeCrit > 0)
        {
            description += "치명타 : +" + changeCrit + " ";
        }
        if (changeAccuracy > 0)
        {
            description += "명중 : +" + changeAccuracy + " ";
        }
        if (changeEvasion > 0)
        {
            description += "회피 : +" + changeEvasion + " ";
        }
        if (changeResist > 0)
        {
            description += "저항 : +" + changeResist + " ";
        }
        if (changeMinStat > 0)
        {
            description += "최소 스탯 : +" + changeMinStat + " ";
        }
        if (changeMaxStat > 0)
        {
            description += "최대 스탯 : +" + changeMaxStat + " ";
        }
        if (changeSpeed > 0)
        {
            description += "속도 : +" + changeSpeed + " ";
        }
        description += "\n";
        text.text += description;
        text.color = Color.blue;
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
}