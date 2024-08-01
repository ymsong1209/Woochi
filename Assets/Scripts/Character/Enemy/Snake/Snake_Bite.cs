using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Bite : BaseSkill
{
    public Snake_Bite()
    {
        
    }
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        //스킬이 명중하면 앞으로 1열 이동
        if (SkillResult.isHit)
        {
            BattleManager.GetInstance.MoveCharacter(SkillOwner, 1);
        }
        
    }
}
