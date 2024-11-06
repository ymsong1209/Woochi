using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_TreeBind_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "뿌리 속박+";
        statDeBuff.BuffDurationTurns = 4;
        statDeBuff.ChanceToApplyBuff = 100;
        statDeBuff.changeStat.SetValue(StatType.Speed, -2);
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
        text.text = "옭아매기+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "100%의 확률로 4턴 동안 속도,최소,최대스탯 -2만큼 부여";
    }
}
