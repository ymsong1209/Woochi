using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tiger_Rip : BaseSkill
{
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        float RandomStat = Random.Range(SkillOwner.FinalStat.minStat, SkillOwner.FinalStat.maxStat);
        RandomStat *= (Multiplier / 100);
        RandomStat = RandomStat * (1 - receiver.FinalStat.defense/(receiver.FinalStat.defense + 100));
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
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "찢어발기기\n" + "대상에게 " + SkillSO.BaseMultiplier +"%의 피해를 주고 출혈 부여\n출혈 상태인 적에게 추가 피해";
    }
    
}
