using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunDeBuff : BaseBuff
{

    public StunDeBuff()
    {
        buffEffect = BuffEffect.Stun;
        buffType = BuffType.Negative;
    }

    public override int ApplyTurnStartBuff()
    {
        --buffDurationTurns;
        Debug.Log(buffOwner.name + "is Stunned. Stun leftover turn : " + buffDurationTurns.ToString());
        //턴이 스킵되었으므로 -1 반환
        return -1;
    }
    
}