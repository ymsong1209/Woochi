using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_MuddyWater : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject PoisonPrefab = BuffPrefabList[0];
        GameObject poisonGameObject = Instantiate(PoisonPrefab, transform);
        PoisonBuff poisonDebuff = poisonGameObject.GetComponent<PoisonBuff>();
        poisonDebuff.ChanceToApplyBuff = 60;
        poisonDebuff.PoisonStack = 3;
        instantiatedBuffList.Add(poisonGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "흙탕물\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "60%의 확률로 단일 대상에게 중독 디버프 3만큼 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "60% 확률로 단일 대상에게\n" +
                                "중독 3 부여";
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
                                "60% 확률로 단일 대상에게\n" +
                                "중독 3 부여\n" + 
                                "-\n" +
                                "도력 " + enhancedMainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "<color=#FFFF00>80</color>% 확률로 단일 대상에게\n" +
                                "중독 <color=#FFFF00>5</color> 부여";
    }
}
