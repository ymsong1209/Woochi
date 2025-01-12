using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MoveResistBuff : BaseBuff
{
    public MoveResistBuff()
    {
        buffEffect = BuffEffect.MoveResist;
        buffType = BuffType.Positive;
        buffStackType = BuffStackType.ResetDuration;
    }
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "강제 이동에 면역";
        text.text = description;
        SetBuffColor(text);
    }
}
