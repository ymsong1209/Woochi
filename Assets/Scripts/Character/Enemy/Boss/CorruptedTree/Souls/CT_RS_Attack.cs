using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_RS_Attack : BaseSkill
{
    
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        GameObject PoisonPrefab = BuffPrefabList[0];
        GameObject poisonGameObject = Instantiate(PoisonPrefab, transform);
        PoisonBuff poisonDebuff = poisonGameObject.GetComponent<PoisonBuff>();
        poisonDebuff.ChanceToApplyBuff = 35;
        poisonDebuff.PoisonStack = 3;
        instantiatedBuffList.Add(poisonGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        Stat finalStat = SkillOwner.FinalStat;
        float RandomStat = Random.Range(finalStat.GetValue(StatType.MinDamage), finalStat.GetValue(StatType.MaxDamage));
        RandomStat *= (Multiplier / 100);
        //귀목의 저주가지는 방어력을 무시하고 대미지를 준다.
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
}
