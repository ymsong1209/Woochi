using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fascinate_Row4_StunDebuff : StunDeBuff
{
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.baseBuffDurationTurns = 1;
        base.AddBuff(_buffOwner);
    }
}
