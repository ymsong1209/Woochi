using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 3턴 동안 대상자는 매 턴 마다 최대 체력의 bleedpercent%의 데미지를 입는다.
/// 출혈 디버프가 있는 동안 다시 출혈에 걸리면 중첩이 올라가고
/// 최대 체력 * bleedpercent / 100 %의 데미지를 준다.
/// </summary>
public class BleedDeBuff : BaseBuff
{
  
    //출혈 중첩 수
    [SerializeField,ReadOnly] private int bleedPercent = 0;
    public override int ApplyTurnStartBuff()
    {
        //전체체력에서 bleedApply%만큼 피를 깎는다.
        float bleedDamage = buffOwner.Health.MaxHealth * bleedPercent / 100f;
        buffOwner.Health.ApplyDamage((int)Mathf.Round(bleedDamage));

        --buffDurationTurns;

        return (int)Mathf.Round(bleedDamage);
    }
    
    //출혈 스택이 쌓일 경우 buffdurationturn과 bleedpercent이 중첩됨
    public override void StackBuff(BaseBuff _buff)
    {
        base.buffDurationTurns += _buff.BuffDurationTurns;
        base.buffBattleDurationTurns += _buff.BuffBattleDurationTurns;
        BleedDeBuff bleedDeBuff = _buff as BleedDeBuff;
        bleedPercent += bleedDeBuff.BleedPercent;
    }

    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "출혈" + buffDurationTurns+ " : 매턴마다 최대 체력의 " + bleedPercent + "% 만큼 피해를 입습니다.";
        text.text = description;
        SetBuffColor(text);
    }
    
    public BleedDeBuff()
    {
        buffEffect = BuffEffect.Bleed;
        buffType = BuffType.Negative;
    }
    
    public int BleedPercent
    {
        get
        {
        return bleedPercent;
        }
        set
        {
            bleedPercent = value;
        }
    }
}
