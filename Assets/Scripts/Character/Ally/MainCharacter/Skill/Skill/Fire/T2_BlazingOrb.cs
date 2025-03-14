using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T2_BlazingOrb : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject BurnPrefab = BuffPrefabList[0];
        GameObject burnGameObject = Instantiate(BurnPrefab, transform);
        BurnDebuff burnDebuff = burnGameObject.GetComponent<BurnDebuff>();
        burnDebuff.BuffDurationTurns = 3;
        burnDebuff.ChanceToApplyBuff = 60;
        instantiatedBuffList.Add(burnGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "불망울\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "랜덤한 대상 2명에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "60%의 확률로 화상 부여";
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
                                "60%의 확률로 화상 부여";
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
                                "60%의 확률로 화상 부여\n" + 
                                "-\n" +
                                "도력 <color=#FFFF00>" + enhancedMainCharacterSkillSo.RequiredSorceryPoints + "</color>을 소모\n" +
                                "랜덤한 대상 2명에게  <color=#FFFF00>" + enhancedMainCharacterSkillSo.BaseMultiplier + "</color>% 피해\n" +
                                "<color=#FFFF00>70</color>%의 확률로 화상 부여";
    }
}