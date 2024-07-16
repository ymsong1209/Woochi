using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(StatBuff))]
public class BuffCharm : BaseCharm
{
    #region 변화된 스탯들의 수치
    [SerializeField] protected Stat changeStat;
    #endregion 변화된 스탯들 frame update

    public override void Activate(BaseCharacter opponent)
    {
        StatBuff buff = GetComponent<StatBuff>();
        buff.StatBuffName = CharmName;
        buff.BuffDurationTurns = Turns;
        buff.ChangeStat = changeStat;
        opponent.ApplyBuff(opponent, buff);
    }
    
}
