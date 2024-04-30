using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Roar_Buff : BaseBuff
{
    public override void AddBuff(BaseCharacter _buffOwner)
    {
        Debug.Log("TigerRoarBuff Added");
        base.buffDurationTurns = 3;

        //방어를 5 증가시킨다.
        changeDefense = 5;
        _buffOwner.Defense += 5;

        base.AddBuff(_buffOwner);
    }

    public override void StackBuff()
    {
        base.buffDurationTurns += 3;
        changeDefense += 5;
        buffOwner.Defense += 5;
    }

    public override bool RemoveBuff()
    {
        buffOwner.Defense -= changeDefense;
        return base.RemoveBuff();
    }

}
