using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_FireSpark_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject BurnPrefab = BuffPrefabList[0];
        GameObject burnGameObject = Instantiate(BurnPrefab, transform);
        BurnDebuff burnDebuff = burnGameObject.GetComponent<BurnDebuff>();
        burnDebuff.BuffDurationTurns = 3;
        burnDebuff.ChanceToApplyBuff = 90;
        instantiatedBuffList.Add(burnGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "불티+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "90%의 확률로 화상 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "단일 대상에게 " + SkillSO.BaseMultiplier + "%피해\n" +
                                "90%의 확률로 화상 부여";
    }
}