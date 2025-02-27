using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tiger_Rip_P : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject bleedDebuffPrefab = BuffPrefabList[0];
        GameObject bleedDebuffGameObject = Instantiate(bleedDebuffPrefab, transform);
        BleedDeBuff bleedDebuff = bleedDebuffGameObject.GetComponent<BleedDeBuff>();
        bleedDebuff.BuffDurationTurns = 3;
        bleedDebuff.BleedPercent = 3;
        bleedDebuff.ChanceToApplyBuff = 80;
        instantiatedBuffList.Add(bleedDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        Stat finalStat = SkillOwner.FinalStat;

        float RandomStat = Random.Range(finalStat.GetValue(StatType.MinDamage), finalStat.GetValue(StatType.MaxDamage));
        RandomStat *= (Multiplier / 100);

        float defense = receiver.FinalStat.GetValue(StatType.Defense);
        RandomStat = RandomStat * (1 - defense / (defense + 100));
        //적에게 출혈 버프가 붙어있으면 2배의 대미지
        bool hasBleed = false;
        foreach(BaseBuff buff in receiver.activeBuffs)
        {
            if(buff.BuffEffect == BuffEffect.Bleed)
            {
                hasBleed = true;
            }
        }
        if (hasBleed) RandomStat *= 2f;
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "찢어발기기+\n" + "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고 50%의 확률로 출혈 부여\n" + "출혈 상태인 적에게 2배의 피해";
    }
    
}
