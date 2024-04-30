using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Roar : BaseSkill
{
    [SerializeField] private BaseBuff RoarBuff;
    protected override void ApplyMultiple()
    {
        base.ApplyMultiple();
        ApplyBuff(SkillOwner,RoarBuff);
    }
    
}
