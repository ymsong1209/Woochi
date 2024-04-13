using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class StunDeBuff : BaseBuff
{

    //stun���� ����. �÷��̾��� ���� �ǳʶٰ� �ϰ� ����.
    public override bool ApplyTurnStartBuff()
    {
        --buffDurationTurns;
        Debug.Log(buffOwner.name + "is Stunned. Stun leftover turn : " + buffDurationTurns.ToString());
        return false;
    }

    public override void StackBuff()
    {
        Debug.Log("StunBuff Stacked.");
        base.StackBuff();
    }
}