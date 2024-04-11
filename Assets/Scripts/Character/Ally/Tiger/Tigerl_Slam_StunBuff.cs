using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Skill_Slam_StunBuff : StunBuff
{
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.baseBuffDurationTurns = 1;
        base.AddBuff(_buffOwner);
    }
}
