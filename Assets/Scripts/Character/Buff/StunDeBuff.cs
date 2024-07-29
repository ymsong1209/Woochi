using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class StunDeBuff : BaseBuff
{

    public StunDeBuff()
    {
        buffEffect = BuffEffect.Stun;
        buffType = BuffType.Negative;
    }

    public override int ApplyTurnStartBuff()
    {
        --buffDurationTurns;
        Debug.Log(buffOwner.name + "is Stunned. Stun leftover turn : " + buffDurationTurns.ToString());
        //턴이 스킵되었으므로 -1 반환
        return -1;
    }
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "아무런 행동을 취할 수 없다";
        text.text = description;
        SetBuffColor(text);
    }
}