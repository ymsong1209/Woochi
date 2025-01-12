using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StunResistBuff : BaseBuff
{
    public StunResistBuff()
    {
        buffEffect = BuffEffect.StunResist;
        buffType = BuffType.Positive;
        buffStackType = BuffStackType.ResetDuration;
    }
    public override bool CanApplyBuff(BaseBuff buff)
    {
        if (buff.BuffEffect == BuffEffect.Stun)
        {
            return false;
        }
        return true;
    }
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "기절에 면역";
        text.text = description;
        SetBuffColor(text);
    }
}
