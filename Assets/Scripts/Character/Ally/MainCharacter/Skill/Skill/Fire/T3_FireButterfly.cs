using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T3_FireButterfly : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "갑옷 융해";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 70;
        statDeBuff.changeStat.SetValue(StatType.Defense, -10);
        instantiatedBuffList.Add(statDebuffGameObject);

        BaseCharacter opponent = BattleUtils.FindRandomEnemy(this);
        base.ActivateSkill(opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "화접\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "랜덤한 대상 2명에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "70%의 확률로 2턴동안 방어 -510큼 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "랜덤한 대상 2명에게 " + SkillSO.BaseMultiplier + "%피해\n" +
                                "70%의 확률로 2턴동안 방어 -10 부여";
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
                                "랜덤한 대상 2명에게 " + SkillSO.BaseMultiplier + "%피해\n" +
                                "70%의 확률로 2턴동안 방어 -10 부여\n" + 
                                "-\n" +
                                "도력 <color=#FFFF00>" + enhancedMainCharacterSkillSo.RequiredSorceryPoints + "</color>을 소모\n" +
                                "랜덤한 대상 2명에게 <color=#FFFF00>" + enhancedMainCharacterSkillSo.BaseMultiplier + "</color>% 피해\n" +
                                "<color=#FFFF00>90</color>%의 확률로 <color=#FFFF00>3</color>턴동안 방어 -10 부여";
    }
}