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
        buffStackType = BuffStackType.ResetDuration;
        buffDurationTurns = 1;
    }

    public override int ApplyTurnStartBuff()
    {
        --buffDurationTurns;
        Logger.BattleLog($"\"{buffOwner.Name}\"({buffOwner.RowOrder + 1}열)은 기절 상태입니다, 남은 기절 턴 : {buffDurationTurns}", "기절버프");
        //Debug.Log(buffOwner.name + "is Stunned. Stun leftover turn : " + buffDurationTurns.ToString());
        //턴이 스킵되었으므로 -1 반환
        return -1;
    }
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description;
        if (buffDurationTurns > -1)
        {
            description = "아무런 행동을" + buffDurationTurns + "턴 만큼 취할 수 없다";
        }
        else
        {
            description = "아무런 행동을 취할 수 없다";
        }
        text.text = description;
        SetBuffColor(text);
    }
}