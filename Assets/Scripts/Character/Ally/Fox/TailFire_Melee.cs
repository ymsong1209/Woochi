using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailFire_Melee : BaseSkill
{
    // ReSharper disable Unity.PerformanceAnalysis
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        BattleManager.GetInstance.MoveCharacter(SkillOwner, -1);
    }
    
}