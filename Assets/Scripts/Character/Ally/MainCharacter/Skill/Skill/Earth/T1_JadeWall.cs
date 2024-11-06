using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_JadeWall : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statbuffPrefab = BuffPrefabList[0];
        GameObject statbuffGameObject = Instantiate(statbuffPrefab, transform);
        StatBuff statBuff = statbuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "옥벽";
        statBuff.BuffDurationTurns = 2;
        statBuff.IsAlwaysApplyBuff = true;
        statBuff.changeStat.SetValue(StatType.Defense, 5);
        instantiatedBuffList.Add(statbuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "옥벽\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "우치 자신에게 2턴동안 방어력 5만큼 부여";
    }
}