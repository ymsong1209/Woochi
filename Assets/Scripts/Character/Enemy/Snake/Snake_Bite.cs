using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Bite : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        BattleManager.GetInstance.MoveCharacter(SkillOwner, 1);
    }
}
