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
        bleedDebuff.ChanceToApplyBuff = 50;
        instantiatedBuffList.Add(bleedDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        float RandomStat = Random.Range(SkillOwner.FinalStat.minStat, SkillOwner.FinalStat.maxStat);
        RandomStat *= (Multiplier / 100);
        RandomStat = RandomStat * (1 - receiver.FinalStat.defense/(receiver.FinalStat.defense + 100));
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
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "찢어발기기+\n" + "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고 50%의 확률로 출혈 부여\n" + "출혈 상태인 적에게 2배의 피해";
    }
    
}
