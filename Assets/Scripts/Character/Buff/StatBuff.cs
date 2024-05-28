using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatBuff : BaseBuff
{
    public string StatBuffName;

    public override void AddBuff(BaseCharacter _buffOwner)
    {
       base.AddBuff(_buffOwner);
       buffOwner.CheckForStatChange();
    }
    
    public override void StackBuff(BaseBuff _buff)
    {
        //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        if(_buff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        else base.buffDurationTurns += _buff.BuffDurationTurns;
        
        base.changeAccuracy += _buff.ChangeAccuracy;
        base.changeSpeed += _buff.ChangeSpeed;
        base.changeDefense += _buff.ChangeDefense;
        base.changeCrit += _buff.ChangeCrit;
        base.changeEvasion += _buff.ChangeEvasion;
        base.changeResist += _buff.ChangeResist;
        base.changeMinStat += _buff.ChangeMinStat;
        base.changeMaxStat += _buff.ChangeMaxStat;
        buffOwner.CheckForStatChange();
    }

    public override void RemoveBuff()
    {
        buffOwner.CheckForStatChange();
        base.RemoveBuff();
    }
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description =  StatBuffName + BuffDurationTurns+": ";
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
}