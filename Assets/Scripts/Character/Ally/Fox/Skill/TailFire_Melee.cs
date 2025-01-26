using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 1,2열일때는 적 1,2열 대상으로 타격
/// 타격 성공시 뒤로 1열 이동
/// 근거리 스킬
/// </summary>
public class TailFire_Melee : BaseSkill
{
    // ReSharper disable Unity.PerformanceAnalysis
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        if (SkillResult.IsAnyHit())
        {
            BattleManager.GetInstance.MoveCharacter(SkillOwner, -1);
        }
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "꼬리불(근거리)\n" + 
                    "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" + 
                    "뒤로 한 칸 이동";
    }
}