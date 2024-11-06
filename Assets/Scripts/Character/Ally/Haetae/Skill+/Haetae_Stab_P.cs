using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
    
public class Haetae_Stab_P : BaseSkill
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
        Stat finalStat = SkillOwner.FinalStat;
        float RandomStat = Random.Range(finalStat.GetValue(StatType.MinDamage), finalStat.GetValue(StatType.MaxDamage));
        RandomStat *= (Multiplier / 100);
        //심판의 뿔은 방어력을 무시하고 대미지를 준다.
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "심판의 뿔+\n" + 
                    "대상의 방어력을 무시하고 " + minStat + " ~ " + maxStat + "의 피해를 줌\n" + 
                    "40%의 확률로 기절 부여";
    }
}
