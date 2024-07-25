using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StatDeBuff))]
public class DebuffCharm : BaseCharm
{
    #region 변화된 스탯들의 수치
    [SerializeField] protected Stat changeStat;
    #endregion 변화된 스탯들 frame update

    public override void Activate(BaseCharacter opponent)
    {
        StatDeBuff buff = GetComponent<StatDeBuff>();
        buff.BuffName = CharmName;
        buff.BuffDurationTurns = Turns;
        buff.ChangeStat = changeStat;
        BaseCharacter caster = BattleManager.GetInstance.currentCharacter;
        if (caster.IsMainCharacter)
        {
            opponent.ApplyBuff(caster, opponent, buff);
        }
    }

}
