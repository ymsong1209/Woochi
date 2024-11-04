using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_HealWater_P : MainCharacterSkill
{
    private int healamount = 5;
    
    protected override float CalculateHeal(BaseCharacter receiver, bool isCrit)
    {
        int heal = healamount;
        if (isCrit) heal = heal * 2;
        return heal;
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "약수+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상을 " + healamount +  "만큼 회복";
    }
    
    public int HealAmount
    {
        get => healamount;
    }
}
