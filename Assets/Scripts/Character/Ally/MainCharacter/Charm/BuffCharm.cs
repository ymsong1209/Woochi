using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatBuff))]
public class BuffCharm : BaseCharm
{
   #region 변화된 스탯들의 수치
    [SerializeField] protected float changeDefense;
    [SerializeField] protected float changeCrit;
    [SerializeField] protected float changeAccuracy;
    [SerializeField] protected float changeEvasion;
    [SerializeField] protected float changeResist;
    [SerializeField] protected float changeMinStat;
    [SerializeField] protected float changeMaxStat;
    [SerializeField] protected float changeSpeed;
    #endregion 변화된 스탯들 frame update

    public override void Activate(BaseCharacter opponent)
    {
        StatBuff buff = GetComponent<StatBuff>();
        buff.StatBuffName = CharmName;
        buff.BuffDurationTurns = Turns;
        buff.ChangeDefense = changeDefense;
        buff.ChangeCrit = changeCrit;
        buff.ChangeAccuracy = changeAccuracy;
        buff.ChangeEvasion = changeEvasion;
        buff.ChangeResist = changeResist;
        buff.ChangeMinStat = changeMinStat;
        buff.ChangeMaxStat = changeMaxStat;
        buff.ChangeSpeed = changeSpeed;
        opponent.ApplyBuff(opponent, buff);
    }
    
    
}
