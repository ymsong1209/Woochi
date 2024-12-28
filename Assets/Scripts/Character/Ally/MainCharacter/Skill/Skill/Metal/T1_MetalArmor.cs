using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_MetalArmor : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statbuffPrefab = BuffPrefabList[0];
        GameObject statbuffGameObject = Instantiate(statbuffPrefab, transform);
        StatBuff statBuff = statbuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "철갑";
        statBuff.BuffDurationTurns = 3;
        statBuff.ChanceToApplyBuff = 100;
        statBuff.changeStat.SetValue(StatType.Defense, 5);
        instantiatedBuffList.Add(statbuffGameObject);
        
        base.ActivateSkill(_opponent);
    }

    protected override void ApplySkillSingleWithoutSelf(BaseCharacter opponent)
    {
        ApplySkill(opponent);
        ApplySkill(SkillOwner);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "철갑\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상과 우치에게 3턴동안 방어력 5만큼 부여";
    }
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "단일 대상과 우치에게\n" +
                                "3턴동안 방어 5 부여";
    }
    
    public override void SetEnhancedSkillScrollDescription(int curskillid, TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        int enhancedSkillID = GameManager.GetInstance.Library.GetEnhancedSkillID(curskillid);
        MainCharacterSkill enhancedSkill = GameManager.GetInstance.Library.GetSkill(enhancedSkillID) as MainCharacterSkill;
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        MainCharacterSkillSO enhancedMainCharacterSkillSo = enhancedSkill.SkillSO as MainCharacterSkillSO;
        
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "단일 대상과 우치에게\n" +
                                "3턴동안 방어력 5만큼 부여\n" + 
                                "-\n"+
                                "도력 <color=#FFFF00>" + enhancedMainCharacterSkillSo.RequiredSorceryPoints + "</color>을 소모\n" +
                                "단일 대상과 우치에게\n" +
                                "3턴동안 방어 <color=#FFFF00>10</color> 부여";
    }
}
