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
                opponent.ApplyBuff(buff.Caster, opponent, buff);
                SkillOwner.RemoveBuffAtIndex(i);
                IncreaseStat();
            }
        }
    }

    private void IncreaseStat()
    {
        statBuff.BuffName = "부정한 율법";
        statBuff.BuffDurationTurns = -1;
        statBuff.ChangeStat.accuracy = 1;
        statBuff.ChangeStat.crit = 1;
        statBuff.ChangeStat.evasion = 1;
        statBuff.ChangeStat.minStat = 1;
        statBuff.ChangeStat.maxStat = 1;
        statBuff.ChangeStat.speed = 1;
        statBuff.ChangeStat.defense = 1;
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,statBuff);
        SkillOwner.CheckForStatChange();
    }
    
    
}
