using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Shout_DeBuff : BaseBuff
{
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.buffDurationTurns = 2;

        leftoverAccuracy = 5;
        buffOwner.Defense = ExecuteLeftOverStatReduction(buffOwner.Defense, ref leftoverAccuracy);
        leftoverMinStat = 5;
        buffOwner.MinStat = ExecuteLeftOverStatReduction(buffOwner.MinStat, ref leftoverMinStat);
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
