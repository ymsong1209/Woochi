using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_HeatHaze_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statBuffPrefab = BuffPrefabList[0];
        GameObject statBuffGameObject = Instantiate(statBuffPrefab, transform);
        StatBuff statBuff = statBuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "아지랑이+";
        statBuff.BuffDurationTurns = -1;
        statBuff.IsAlwaysApplyBuff = true;
        statBuff.BuffStackType = BuffStackType.ResetDuration;
        statBuff.changeStat.SetValue(StatType.Evasion, 35);
        instantiatedBuffList.Add(statBuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "아지랑이+\n" +
                    "도력 " + requiredSorceryPoints + "을 소모하여\n" +
                    "우치 자신에게 회피를 이번 전투동안 35만큼 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "우치에게 이번 전투동안\n" +
                                "회피 35 부여";
    }
}