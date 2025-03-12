using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tiger_Bite : BaseSkill
{
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject bleedDebuffPrefab = BuffPrefabList[0];
        GameObject bleedDebuffGameObject = Instantiate(bleedDebuffPrefab, transform);
        BleedDeBuff bleedDebuff = bleedDebuffGameObject.GetComponent<BleedDeBuff>();
        bleedDebuff.BuffDurationTurns = 3;
        bleedDebuff.BleedPercent = 3;
        bleedDebuff.ChanceToApplyBuff = 50;
        instantiatedBuffList.Add(bleedDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        
        // 물어뜯기로 피해를 입히면 적의 잃은 체력의 30% 만큼 피해를 줌
        // 이 추가 피해는 적 최대 체력의 15%를 넘을 수 없음
        Health opponentHealth = receiver.Health;
        float Damage = (opponentHealth.MaxHealth - opponentHealth.CurHealth) * 0.3f;
        Damage = Mathf.Min(Damage, opponentHealth.MaxHealth * 0.15f);

        return Damage;
    }

    protected override int ApplyStat(BaseCharacter receiver, bool isCrit)
    {
        int Damage = base.ApplyStat(receiver, isCrit);
        //호랑이는 준 피해의 50%만큼 회복함.
        int healamount = (int)Mathf.Round(Damage * 0.5f);
        SkillOwner.Health.Heal(healamount);
        return Damage;
    }

    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "물어뜯기\n" +
                    "잃은 체력 비례 최대 30%의 피해를 주고\n" +
                    "피해의 50%만큼 회복\n" +
                    "3턴동안 50%의 확률로 출혈 3턴 부여";
    }
    
}
