using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T3_DisasterRain_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        GameObject PoisonPrefab = BuffPrefabList[0];
        GameObject poisonGameObject = Instantiate(PoisonPrefab, transform);
        PoisonBuff poisonDebuff = poisonGameObject.GetComponent<PoisonBuff>();
        poisonDebuff.ChanceToApplyBuff = 90;
        poisonDebuff.PoisonStack = 3;
        instantiatedBuffList.Add(poisonGameObject);
        
        GameObject statDebuffPrefab = BuffPrefabList[1];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "약화+";
        statDeBuff.BuffDurationTurns = 3;
        statDeBuff.ChanceToApplyBuff = 90;
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
        text.text = "삼재의 비+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" +
                    "1,2,3열의 적에게\n" +
                    "90%의 확률로 중독 3, 3턴동안 최소,최대스탯 -3만큼 부여";
    }
}