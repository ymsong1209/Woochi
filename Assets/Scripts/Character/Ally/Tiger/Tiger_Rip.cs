using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Rip : BaseSkill
{
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        float RandomStat = Random.Range(SkillOwner.Stat.minStat, SkillOwner.Stat.maxStat);
        RandomStat *= (Multiplier / 100);
        RandomStat = RandomStat * (1 - receiver.Stat.defense/(receiver.Stat.defense + 100));
        //적에게 출혈 버프가 붙어있으면 1.5배의 대미지
        bool hasBleed = false;
        foreach(BaseBuff buff in receiver.activeBuffs)
        {
            if(buff.BuffEffect == BuffEffect.Bleed)
            {
                hasBleed = true;
            }
        }
        if (hasBleed) RandomStat *= 1.5f;
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
}
