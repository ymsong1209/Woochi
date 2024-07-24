using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxFire_Row4_CureBuff : BaseBuff
{
    private BaseCharacter fox;
    public override void AddBuff(BaseCharacter caster, BaseCharacter _buffOwner)
    {
        base.buffDurationTurns = 3;
        base.AddBuff(caster, _buffOwner);
    }
    
    public override int ApplyTurnStartBuff()
    {
        Stat stat = fox.Stat;
        //삼미호의 피해의 20%만큼 힐을 해야함
        float RandomStat = Random.Range(stat.minStat, stat.maxStat) * 20f / 100f;
        buffOwner.Health.Heal((int)RandomStat);

        --buffDurationTurns;

        return 0;
    }
    public override void StackBuff(BaseBuff _buff)
    {
        //버프 지속시간 3으로 맞춤.
        base.buffDurationTurns = 3;
    }

    public void SetFox(BaseCharacter _fox)
    {
        fox = _fox;
    }
}
