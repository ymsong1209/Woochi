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
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모하여\n" +
                                "단일 대상을 " + healamount +  "만큼 회복";
    }
    
    public override void SetEnhancedSkillScrollDescription(int curskillid, TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        int enhancedSkillID = GameManager.GetInstance.Library.GetEnhancedSkillID(curskillid);
        T1_HealWater_P enhancedSkill = GameManager.GetInstance.Library.GetSkill(enhancedSkillID) as T1_HealWater_P;
        MainCharacterSkillSO mainCharacterSkillSo = enhancedSkill.SkillSO as MainCharacterSkillSO;
        
        skillDescription.text = "도력 <color=#FFFF00>" + mainCharacterSkillSo.RequiredSorceryPoints + "</color>을 소모하여\n" +
                                "단일 대상을 <color=#FFFF00>" +  enhancedSkill.HealAmount + "</color>만큼 회복";
    }
}
