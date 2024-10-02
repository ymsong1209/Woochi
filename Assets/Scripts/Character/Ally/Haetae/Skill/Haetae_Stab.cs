using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 1,2열중 단일 대상으로 타격
/// </summary>
public class Haetae_Stab : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject stunDebuffPrefab = BuffPrefabList[0];
        GameObject stunDebuffGameObject = Instantiate(stunDebuffPrefab, transform);
        StunDeBuff stunDebuff = stunDebuffGameObject.GetComponent<StunDeBuff>();
        stunDebuff.BuffDurationTurns = 1;
        stunDebuff.ChanceToApplyBuff = 40;
        instantiatedBuffList.Add(stunDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        float RandomStat = Random.Range(SkillOwner.FinalStat.minStat, SkillOwner.FinalStat.maxStat);
        RandomStat *= (Multiplier / 100);
        //심판의 뿔은 방어력을 무시하고 대미지를 준다.
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "심판의 뿔\n" + 
                    "대상의 방어력을 무시하고 " + minStat + " ~ " + maxStat + "의 피해를 줌\n" + 
                    "40%의 확률로 기절 부여";
    }
}
