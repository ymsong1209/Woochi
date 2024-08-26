using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanseSingleDebuffCharm : BaseCharm
{
   
    public override void Activate(BaseCharacter opponent)
    {
        foreach(BaseBuff buff in opponent.activeBuffs)
        {
            if(buff.BuffType == BuffType.Negative && buff.IsRemovableDuringBattle)
            {
                opponent.RemoveBuff(buff);
                break;
            }
        }
    }
}
