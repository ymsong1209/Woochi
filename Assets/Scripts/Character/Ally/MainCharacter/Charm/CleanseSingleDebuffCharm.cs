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
                Logger.BattleLog($"\"{opponent.Name}\"({opponent.RowOrder + 1}열)의 \"{buff.BuffName}\" 제거", "정화 부적");
                opponent.RemoveBuff(buff);
                break;
            }
        }
    }
}
