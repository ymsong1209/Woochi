using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailFire_Remote : BaseSkill
{
    public override void ApplySkill(BaseCharacter _Opponent)
    {
        base.ApplySkill(_Opponent);

        BattleManager.GetInstance.MoveCharacter(SkillOwner, -1);
    }
}
