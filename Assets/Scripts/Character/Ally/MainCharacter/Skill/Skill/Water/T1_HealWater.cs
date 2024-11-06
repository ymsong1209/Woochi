using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_HealWater : MainCharacterSkill
{
    private int healamount = 3;
    
    protected override float CalculateHeal(BaseCharacter receiver, bool isCrit)
    {
        int heal = healamount;
        if (isCrit) heal = heal * 2;
        return heal;
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "약수\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상을 " + healamount +  "만큼 회복";
    }
}
