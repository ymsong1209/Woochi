using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ChangeLocation : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        BattleManager.GetInstance.MoveCharacter(SkillOwner, _Opponent);
    }
}
