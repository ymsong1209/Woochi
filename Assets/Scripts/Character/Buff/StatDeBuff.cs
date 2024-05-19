using System.Collections;
using System.Collections.Generic;
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
}