using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_MetalArmor_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statbuffPrefab = BuffPrefabList[0];
        GameObject statbuffGameObject = Instantiate(statbuffPrefab, transform);
        StatBuff statBuff = statbuffGameObject.GetComponent<StatBuff>();
        statBuff.BuffName = "철갑+";
        statBuff.BuffDurationTurns = 3;
        statBuff.IsAlwaysApplyBuff = true;
        statBuff.changeStat.defense = 8;
        instantiatedBuffList.Add(statbuffGameObject);
        
        base.ActivateSkill(_opponent);
    }

    protected override void ApplySkillSingleWithoutSelf(BaseCharacter opponent)
    {
        ApplySkill(opponent);
        ApplySkill(SkillOwner);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "철갑+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상과 우치에게 3턴동안 방어 8만큼 부여";
    }
}
