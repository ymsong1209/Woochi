using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Skill_Slam_StunBuff : StunDeBuff
{
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        Debug.Log("Tiger Slam Debuff added");
        base.baseBuffDurationTurns = 1;
        base.AddBuff(_buffOwner);
    }
    
    
}
