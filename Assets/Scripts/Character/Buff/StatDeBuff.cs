using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDeBuff : BaseBuff
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
    public StatDeBuff()
    {
        buffEffect = BuffEffect.StatStrengthen;
        buffType = BuffType.Negative;
    }
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.AddBuff(_buffOwner);
        buffOwner.CheckForStatChange();
    }
    
    public override void StackBuff(BaseBuff _buff)
    {
        StatDeBuff statDeBuff = _buff as StatDeBuff;
        if (!statDeBuff) return;
        //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        if(_buff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        else base.buffDurationTurns += _buff.BuffDurationTurns;
        
        changeAccuracy += statDeBuff.ChangeAccuracy;
        changeSpeed += statDeBuff.ChangeSpeed;
        changeDefense += statDeBuff.ChangeDefense;
        changeCrit += statDeBuff.ChangeCrit;
        changeEvasion += statDeBuff.ChangeEvasion;
        changeResist += statDeBuff.ChangeResist;
        changeMinStat += statDeBuff.ChangeMinStat;
        changeMaxStat += statDeBuff.ChangeMaxStat;
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
        if (ChangeDefense < 0)
        {
            description += "방어력 : " + ChangeDefense + " ";
        }
        if (ChangeCrit < 0)
        {
            description += "치명타 : " + ChangeCrit + " ";
        }
        if (ChangeAccuracy < 0)
        {
            description += "명중 : " + ChangeAccuracy + " ";
        }
        if (ChangeEvasion < 0)
        {
            description += "회피 : " + ChangeEvasion + " ";
        }
        if (ChangeResist < 0)
        {
            description += "저항 : " + ChangeResist + " ";
        }
        if (ChangeMinStat < 0)
        {
            description += "최소 스탯 : " + ChangeMinStat + " ";
        }
        if (ChangeMaxStat < 0)
        {
            description += "최대 스탯 : " + ChangeMaxStat + " ";
        }
        if (ChangeSpeed < 0)
        {
            description += "속도 : " + ChangeSpeed + " ";
        }
        description += "\n";
        text.text += description;
        text.color = Color.red;
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