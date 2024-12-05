using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FearBuff : BaseBuff
{
    private float damageReduction;
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "두려움 : 최종대미지 " + damageReduction + "% 감소";
        text.text = description;
        SetBuffColor(text);
    }
    
    public FearBuff()
    {
        buffEffect = BuffEffect.Fear;
        buffType = BuffType.Negative;
        damageReduction = 20f;
        buffDurationTurns = -1;
    }
    
    public float DamageReduction => damageReduction;
}
