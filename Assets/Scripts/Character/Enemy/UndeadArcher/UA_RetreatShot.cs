using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UA_RetreatShot : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        BattleManager.GetInstance.MoveCharacter(SkillOwner, -1);
    }
}
