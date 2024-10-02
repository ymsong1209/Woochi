using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T2_EarthClone : MainCharacterSkill
{
    [SerializeField] private GameObject statBuffGameObject;
    public override void ActivateSkill(BaseCharacter _opponent)
    {

        base.ActivateSkill(_opponent);
        
        GameObject instantiatedStatbuff = Instantiate(statBuffGameObject, transform);
        StatBuff statBuff = instantiatedStatbuff.GetComponent<StatBuff>();
        statBuff.BuffName = "분신술";
        statBuff.BuffDurationTurns = 3;
        statBuff.IsAlwaysApplyBuff = true;
        statBuff.changeStat.evasion = 4;
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,statBuff);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "흙 분신\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "우치에게 3턴동안 회피 4만큼 부여";
    }
}