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
        poisonDebuff.ChanceToApplyBuff = 100;
        poisonDebuff.PoisonStack = 3;
        instantiatedBuffList.Add(poisonGameObject);
        
        GameObject statDebuffPrefab = BuffPrefabList[1];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "약화";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 100;
        statDeBuff.BuffStackType = BuffStackType.ResetDuration;
        statDeBuff.changeStat.SetValue(StatType.MinDamage, -2);
        statDeBuff.changeStat.SetValue(StatType.MaxDamage, -2);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "[삼재의 비]\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여 " +
                    "1,2,3열의 적에게 " +
                    "100%의 확률로 중독 3, 2턴동안 공격력 <color=#8AAFDC>-2</color> 부여";
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
                                "공격력 -2만큼 부여\n" +
                                "100%의 확률로 중독 3 부여";
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
                                "공격력 -2만큼 부여\n" +
                                "100%의 확률로 중독 3 부여\n" + 
                                "-\n" +
                                "도력 <color=#FFFF00>" + enhancedMainCharacterSkillSo.RequiredSorceryPoints + "</color>을 소모\n" +
                                "1,2,3열의 적에게 <color=#FFFF00>3</color>턴동안\n " +
                                "공격력 -2만큼 부여\n" +
                                "100%의 확률로 중독 <color=#FFFF00>5</color> 부여";
    }
}