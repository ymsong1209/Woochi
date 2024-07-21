using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        BattleManager.GetInstance.MoveCharacter(SkillOwner, -1);
    }
    
}