using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T2_EarthClone_P : MainCharacterSkill
{
    [SerializeField] private GameObject statBuffGameObject;
    public override void ActivateSkill(BaseCharacter _opponent)
    {

        base.ActivateSkill(_opponent);
        
        GameObject instantiatedStatbuff = Instantiate(statBuffGameObject, transform);
        StatBuff statBuff = instantiatedStatbuff.GetComponent<StatBuff>();
        statBuff.BuffName = "분신술+";
        statBuff.BuffDurationTurns = 3;
        statBuff.IsAlwaysApplyBuff = true;
        statBuff.changeStat.SetValue(StatType.Evasion, 8);
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,statBuff);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "흙 분신+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "우치에게 3턴동안 회피 8만큼 부여";
    }
}