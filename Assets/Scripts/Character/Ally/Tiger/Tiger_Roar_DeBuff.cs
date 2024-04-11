using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Shout_DeBuff : BaseBuff
{
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.buffDurationTurns = 2;

        //명중을 5 감소시킨다.
        changeAccuracy = -5;
        leftoverAccuracy = 5;
        buffOwner.Defense = ExecuteLeftOverStatReduction(buffOwner.Defense, ref leftoverAccuracy);

        changeMinStat = -5;
        leftoverMinStat = 5;
        buffOwner.MinStat = ExecuteLeftOverStatReduction(buffOwner.MinStat, ref leftoverMinStat);

        changeMaxStat = -5;
        leftoverMaxStat = 5;
        buffOwner.MaxStat = ExecuteLeftOverStatReduction(buffOwner.MaxStat, ref leftoverMaxStat);

        base.AddBuff(buffOwner);
    }

    public override bool RemoveBuff()
    {

        buffOwner.Defense += 5;
        buffOwner.MinStat += 5;
        buffOwner.MaxStat += 5;

        return base.RemoveBuff();
    }

}
