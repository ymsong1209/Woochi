using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire_Row4_CureBuff : BaseBuff
{
    private BaseCharacter fox;
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.baseBuffDurationTurns = 3;
        base.AddBuff(_buffOwner);
    }
    
    public override bool ApplyTurnStartBuff()
    {
        //삼미호의 피해의 20%만큼 힐을 해야함
        float RandomStat = Random.Range(fox.MinStat, fox.MaxStat) * 20f / 100f;
        buffOwner.Health.Heal((int)RandomStat);

        --buffDurationTurns;
        
        return (!buffOwner.CheckDead());
    }
    public override void StackBuff()
    {
        //버프 지속시간 3으로 맞춤.
        base.buffDurationTurns = 3;
    }

    public void SetFox(BaseCharacter _fox)
    {
        fox = _fox;
    }
}
