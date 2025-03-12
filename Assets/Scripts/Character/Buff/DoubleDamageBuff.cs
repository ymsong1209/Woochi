using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoubleDamageBuff : StatBuff
{
    
    public DoubleDamageBuff()
    {
        changeStat = new Stat();
        buffEffect = BuffEffect.StatStrengthen;
        buffType = BuffType.Positive;
        buffStackType = BuffStackType.ResetDuration;
        buffDurationTurns = 1;
    }
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "";
        description = $"{buffName} : 1턴 동안 피해 * 2.";
        text.text = description;
    }
}
