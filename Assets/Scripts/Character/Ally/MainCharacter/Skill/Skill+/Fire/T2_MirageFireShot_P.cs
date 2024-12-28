using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T2_MirageFireShot_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "시야 차단+";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 70;
        statDeBuff.changeStat.SetValue(StatType.MinDamage, -1);
        statDeBuff.changeStat.SetValue(StatType.MaxDamage, -1);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "신기전+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "70%의 확률로 3턴동안 피해 -1만큼 부여";
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
                                "70%의 확률로 3턴동안\n"+
                                "피해 -1 부여";
    }
}