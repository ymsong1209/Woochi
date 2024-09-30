using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_MetalWind : MainCharacterSkill
{
    [SerializeField] private GameObject DefenseBuffGameObject;
    public override void ActivateSkill(BaseCharacter _opponent)
    {

        base.ActivateSkill(_opponent);
        
        GameObject instantiatedDefensebuff = Instantiate(DefenseBuffGameObject, transform);
        StatBuff defenseBuff = instantiatedDefensebuff.GetComponent<StatBuff>();
        defenseBuff.BuffName = "견고함";
        defenseBuff.BuffDurationTurns = 3;
        defenseBuff.IsAlwaysApplyBuff = true;
        defenseBuff.changeStat.defense = 2;
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,defenseBuff);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "옭아매기\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "80%의 확률로 뿌리 속박 디버프 부여";
    }
}