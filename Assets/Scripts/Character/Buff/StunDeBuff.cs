using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunDeBuff : BaseBuff
{

    //stun관련 버프. 플레이어의 턴을 건너뛰게 하고 싶음.
    public override int ApplyTurnStartBuff()
    {
        --buffDurationTurns;
        Debug.Log(buffOwner.name + "is Stunned. Stun leftover turn : " + buffDurationTurns.ToString());
        //턴이 스킵되었으므로 -1 반환
        return -1;
    }
    
}