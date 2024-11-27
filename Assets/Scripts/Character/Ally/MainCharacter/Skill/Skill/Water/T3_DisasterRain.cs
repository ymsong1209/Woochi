using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T3_DisasterRain : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        GameObject PoisonPrefab = BuffPrefabList[0];
        GameObject poisonGameObject = Instantiate(PoisonPrefab, transform);
        PoisonBuff poisonDebuff = poisonGameObject.GetComponent<PoisonBuff>();
        poisonDebuff.ChanceToApplyBuff = 80;
        poisonDebuff.PoisonStack = 2;
        instantiatedBuffList.Add(poisonGameObject);
        
        GameObject statDebuffPrefab = BuffPrefabList[1];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "약화";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 80;
        statDeBuff.changeStat.SetValue(StatType.MinDamage, -3);
        statDeBuff.changeStat.SetValue(StatType.MaxDamage, -3);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "삼재의 비\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" +
                    "1,2,3열의 적에게\n" +
                    "80%의 확률로 중독 2, 2턴동안 최소,최대스탯 -3만큼 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "1,2,3열의 적에게 2턴동안\n " + 
                                "최소, 최대스탯 -3만큼 부여\n" +
                                "80%의 확률로 중독 2 부여";
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
                                "1,2,3열의 적에게 2턴동안\n " + 
                                "최소, 최대스탯 -3만큼 부여\n" +
                                "80%의 확률로 중독 2 부여\n" + 
                                "-\n" +
                                "도력 <color=#FFFF00>" + enhancedMainCharacterSkillSo.RequiredSorceryPoints + "</color>을 소모\n" +
                                "1,2,3열의 적에게 <color=#FFFF00>3</color>턴동안\n " +
                                "최소, 최대스탯 -3만큼 부여\n" +
                                "<color=#FFFF00>90</color>%의 확률로 중독 <color=#FFFF00>3</color> 부여";
    }
}