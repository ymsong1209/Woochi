using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T2_FloralAroma_P : MainCharacterSkill
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
        statDeBuff.BuffName = "향긋한 꽃내음+";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 90;
        statDeBuff.changeStat.minStat = -4;
        statDeBuff.changeStat.maxStat = -4;
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "향긋한 꽃내음+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "90%의 확률로 중독 3, 2턴동안 최소,최대스탯 -4만큼 부여";
    }
}