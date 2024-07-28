using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSkill : BaseSkill
{
    protected int requiredSorceryPoints;
    
    public int RequiredSorceryPoints
    {
        get
        {
            return requiredSorceryPoints;
        }
    }

    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);

        var animation = SkillOwner.anim as MainCharacterAnimation;
        animation.ShowElement(SkillSO.SkillElement);
    }
}
