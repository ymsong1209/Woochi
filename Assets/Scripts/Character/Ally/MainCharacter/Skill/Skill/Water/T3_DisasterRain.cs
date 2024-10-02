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
        statDeBuff.changeStat.minStat = -3;
        statDeBuff.changeStat.maxStat = -3;
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "삼재의 비\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" +
                    "1,2,3열의 적에게\n" +
                    "80%의 확률로 중독 2, 2턴동안 최소,최대스탯 -3만큼 부여";
    }
}