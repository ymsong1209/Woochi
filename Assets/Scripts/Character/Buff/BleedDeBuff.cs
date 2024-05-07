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
    public override bool ApplyTurnStartBuff()
    {
        //전체체력에서 bleedApply%만큼 피를 깎는다.
        float bleedDamage = buffOwner.Health.MaxHealth * bleedApply / 100f;
        buffOwner.Health.ApplyDamage((int)Mathf.Round(bleedDamage));

        --buffDurationTurns;
        //TODO : BleedApply공식이 바뀔수도 있음.

        Debug.Log(buffOwner.name + "is Bleeding. Bleed leftover turn : " + buffDurationTurns.ToString());

        //checkdead는 캐릭터가 죽었을경우 true 반환
        //ApplyTurnStartBuff는 버프 실행 후 캐릭터가 살았으면 true 반환
        return (!buffOwner.CheckDead());
    }

    public override void StackBuff()
    {
        //TODO : 출혈 스택 쌓일 경우 어떻게 하지?
        Debug.Log("BleedBuff Stacked.");
        //base.StackBuff();
    }
}
