using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_HeatHaze : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statBuffPrefab = BuffPrefabList[0];
        GameObject statBuffGameObject = Instantiate(statBuffPrefab, transform);
        StatBuff statBuff = statBuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "아지랑이";
        statBuff.BuffDurationTurns = -1;
        statBuff.ChanceToApplyBuff = 100;
        statBuff.changeStat.evasion = 4;
        instantiatedBuffList.Add(statBuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "아지랑이\n" +
                    "도력 " + requiredSorceryPoints + "을 소모하여\n" +
                    "우치 자신에게 회피를 4만큼 부여합니다";
    }
}
