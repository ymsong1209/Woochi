using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tiger_Bite : BaseSkill
{
    protected override void ApplyStat(BaseCharacter receiver, bool isCrit)
    {
        float Damage = CalculateDamage(receiver, isCrit);
        Health opponentHealth = receiver.Health;
        // 물어뜯기로 피해를 입히면 적의 잃은 체력의 20% 만큼 추가 피해를 줌
        Damage +=  (opponentHealth.MaxHealth - opponentHealth.CurHealth) * 0.2f;
        Damage = Mathf.Clamp(CalculateElementalDamageBuff(Damage),0,9999);
        //호랑이는 준 피해의 30%만큼 회복함.
        int healamount = (int)Mathf.Round(Damage * 0.3f);
        SkillOwner.Health.Heal(healamount);
        
        opponentHealth.ApplyDamage((int)Mathf.Round(Damage), isCrit);
        receiver.CheckDeadAndPlayAnim();
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "물어뜯기\n" + "대상에게 " + SkillSO.BaseMultiplier +"%의 피해를 주고 \n잃은 체력 비례 데미지, 피해 흡혈";
    }
    
}
