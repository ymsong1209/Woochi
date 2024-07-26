using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySkill : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        Debug.Log("Dummy Skill Used");
        base.ActivateSkill(_opponent);
    }
}
