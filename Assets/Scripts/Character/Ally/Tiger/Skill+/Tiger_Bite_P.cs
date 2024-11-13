using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tiger_Bite_P : BaseSkill
{
    protected override void ApplyStat(BaseCharacter receiver, bool isCrit)
    {
        float Damage = CalculateDamage(receiver, isCrit);
        Health opponentHealth = receiver.Health;
        // 물어뜯기로 피해를 입히면 적의 잃은 체력의 25% 만큼 추가 피해를 줌
        Damage +=  (opponentHealth.MaxHealth - opponentHealth.CurHealth) * 0.25f;
        Damage = Mathf.Clamp(CalculateElementalDamageBuff(Damage),0,9999);
        //호랑이는 준 피해의 30%만큼 회복함.
        int healamount = (int)Mathf.Round(Damage * 0.5f);
        SkillOwner.Health.Heal(healamount);
        
        opponentHealth.ApplyDamage((int)Mathf.Round(Damage), isCrit);
        receiver.CheckDeadAndPlayAnim();
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "물어뜯기+\n" + 
                    "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" + 
                    "잃은 체력 비례 25%의 추가 데미지\n" + 
                    "피해의 50%만큼 회복";
    }
    
}
