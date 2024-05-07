using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fascinate_Row3_Debuff : StatBuff
{
    public Fascinate_Row3_Debuff()
    {
        StatBuffName = "fox_fascinate_row3";
    }
    
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        base.buffDurationTurns = 2;

        //명중을 10 감소시킨다.
        changeAccuracy = -10;
        leftoverAccuracy = 10;
        _buffOwner.Accuracy = ExecuteLeftOverStatReduction(_buffOwner.Accuracy, ref leftoverAccuracy);
        //속도를 10 감소시킨다.
        changeSpeed = -10;
        leftoverSpeed = 10;
        _buffOwner.Speed = ExecuteLeftOverStatReduction(_buffOwner.Speed, ref leftoverSpeed);
        
        base.AddBuff(_buffOwner);
    }

    public override void StackBuff()
    {
        base.buffDurationTurns += 2;
        changeAccuracy += -10;
        leftoverAccuracy += 10;
        buffOwner.Accuracy = ExecuteLeftOverStatReduction(buffOwner.Accuracy, ref leftoverAccuracy);

        changeSpeed += -10;
        leftoverSpeed += 10;
        buffOwner.Speed = ExecuteLeftOverStatReduction(buffOwner.Speed, ref leftoverSpeed);
        
    }

    public override bool RemoveBuff()
    {

        buffOwner.Accuracy -= changeAccuracy;
        buffOwner.Speed -= changeSpeed;

        return base.RemoveBuff();
    }
}
