using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tiger_Bite : BaseSkill
{
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        float Damage =  base.CalculateDamage(receiver, isCrit);
        
        // 물어뜯기로 피해를 입히면 적의 잃은 체력의 20% 만큼 추가 피해를 줌
        // 이 추가 피해는 적 최대 체력의 10%를 넘을 수 없음
        Health opponentHealth = receiver.Health;
        float extraDamage = (opponentHealth.MaxHealth - opponentHealth.CurHealth) * 0.2f;
        extraDamage = Mathf.Min(extraDamage, opponentHealth.MaxHealth * 0.1f);

        Damage += extraDamage;
        
        return Damage;
    }

    protected override int ApplyStat(BaseCharacter receiver, bool isCrit)
    {
        int Damage = base.ApplyStat(receiver, isCrit);
        //호랑이는 준 피해의 40%만큼 회복함.
        int healamount = (int)Mathf.Round(Damage * 0.4f);
        SkillOwner.Health.Heal(healamount);
        return Damage;
    }

    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "물어뜯기\n" + "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "잃은 체력 비례 최대 20%의 추가 데미지\n" + "피해의 40%만큼 회복";
    }
    
}
