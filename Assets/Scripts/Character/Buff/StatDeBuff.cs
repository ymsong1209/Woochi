using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDeBuff : BaseBuff
{
    public string StatBuffName;

    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.AddBuff(_buffOwner);
        buffOwner.CheckForStatChange();
    }
    
    public override void StackBuff(BaseBuff _buff)
    {
        base.buffDurationTurns += _buff.BuffDurationTurns;
        
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
        string description = "";
        description += StatBuffName + BuffDurationTurns+": ";
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
}