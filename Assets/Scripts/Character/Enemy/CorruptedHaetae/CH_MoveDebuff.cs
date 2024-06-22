using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_MoveDebuff : BaseSkill
{
    [SerializeField] private StatBuff statBuff;
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        TransferBuff(SkillResult.Opponent);
    }

    private void TransferBuff(BaseCharacter opponent)
    {
        for (int i = SkillOwner.activeBuffs.Count - 1; i >= 0; i--)
        {
            BaseBuff buff = SkillOwner.activeBuffs[i];
            if (buff.BuffType == BuffType.Negative)
            {
                opponent.ApplyBuff(opponent, buff);
                SkillOwner.RemoveBuffAtIndex(i);
                IncreaseStat();
            }
        }
    }

    private void IncreaseStat()
    {
        statBuff.StatBuffName = "부정한 율법";
        statBuff.BuffDurationTurns = -1;
        statBuff.ChangeAccuracy = 1;
        statBuff.ChangeCrit = 1;
        statBuff.ChangeEvasion = 1;
        statBuff.ChangeMinStat = 1;
        statBuff.ChangeMaxStat = 1;
        statBuff.ChangeSpeed = 1;
        statBuff.ChangeDefense = 1;
        SkillOwner.ApplyBuff(SkillOwner,statBuff);
        SkillOwner.CheckForStatChange();
    }
    
    
}
