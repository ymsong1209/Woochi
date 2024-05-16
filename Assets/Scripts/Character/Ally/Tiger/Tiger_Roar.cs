using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Roar : BaseSkill
{
    [SerializeField] private BaseBuff RoarBuff;

    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        ApplyBuff(_Opponent,RoarBuff);
        Debug.Log("Tiger Roar");
    }
}
