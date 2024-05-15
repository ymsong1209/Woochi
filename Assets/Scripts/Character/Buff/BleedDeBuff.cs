using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedDeBuff : BaseBuff
{
    //내 턴이 시작할때 몇 퍼센트만큼 피를 깎을건지
    [SerializeField,ReadOnly] private int bleedPercent = 0;
    //출혈 디버프를 걸때 몇%만큼 출혈스택을 쌓을 것인지
    [SerializeField] protected int bleedApply;
    //bleed관련 디버프
    public override int ApplyTurnStartBuff()
    {
        //전체체력에서 bleedApply%만큼 피를 깎는다.
        float bleedDamage = buffOwner.Health.MaxHealth * bleedApply / 100f;
        buffOwner.Health.ApplyDamage((int)Mathf.Round(bleedDamage));

        --buffDurationTurns;

        return (int)Mathf.Round(bleedDamage);
    }

    public override void StackBuff()
    {
       
    }
}
