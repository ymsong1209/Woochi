using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Roar_DeBuff : BaseBuff
{
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.buffDurationTurns = 2;

        //명중을 5 감소시킨다.
        changeAccuracy = -5;
        leftoverAccuracy = 5;
        _buffOwner.Accuracy = ExecuteLeftOverStatReduction(_buffOwner.Accuracy, ref leftoverAccuracy);

        changeMinStat = -5;
        leftoverMinStat = 5;
        _buffOwner.MinStat = ExecuteLeftOverStatReduction(_buffOwner.MinStat, ref leftoverMinStat);

        changeMaxStat = -5;
        leftoverMaxStat = 5;
        _buffOwner.MaxStat = ExecuteLeftOverStatReduction(_buffOwner.MaxStat, ref leftoverMaxStat);

        base.AddBuff(_buffOwner);
        Debug.Log("Roar Debuff added");
    }

    public override void StackBuff()
    {
        base.buffDurationTurns += 2;
        changeAccuracy += -5;
        leftoverAccuracy += 5;
        buffOwner.Accuracy = ExecuteLeftOverStatReduction(buffOwner.Accuracy, ref leftoverAccuracy);

        changeMinStat += -5;
        leftoverMinStat += 5;
        buffOwner.MinStat = ExecuteLeftOverStatReduction(buffOwner.MinStat, ref leftoverMinStat);

        changeMaxStat += -5;
        leftoverMaxStat += 5;
        buffOwner.MaxStat = ExecuteLeftOverStatReduction(buffOwner.MaxStat, ref leftoverMaxStat);
        
    }

    public override bool RemoveBuff()
    {

        buffOwner.Accuracy -= changeAccuracy;
        buffOwner.MinStat -= changeMinStat;
        buffOwner.MaxStat -= changeMaxStat;

        return base.RemoveBuff();
    }

}
