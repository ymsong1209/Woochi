using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_MetalWind_P : MainCharacterSkill
{
    [SerializeField] private GameObject DefenseBuffGameObject;
    public override void ActivateSkill(BaseCharacter _opponent)
    {

        base.ActivateSkill(_opponent);
        
        GameObject instantiatedDefensebuff = Instantiate(DefenseBuffGameObject, transform);
        StatBuff defenseBuff = instantiatedDefensebuff.GetComponent<StatBuff>();
        defenseBuff.BuffName = "견고함+";
        defenseBuff.BuffDurationTurns = 3;
        defenseBuff.IsAlwaysApplyBuff = true;
        defenseBuff.changeStat.defense = 4;
        SkillOwner.ApplyBuff(SkillOwner,SkillOwner,defenseBuff);
    }
    
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        float FinalDamage = 4 + SkillOwner.FinalStat.defense * 2;
        if(isCrit) FinalDamage *= 2;
        return FinalDamage;
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "쇠바람+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 4 + " + SkillOwner.FinalStat.defense + "*2 의 방어 기반 고정 피해를 주고\n" +
                    "우치에게 3턴동안 방어력 4만큼 부여";
    }
}