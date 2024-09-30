using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 1,2열중 가장 낮은 체력을 가진 아군, 혹은 랜덤한 아군 1명을 선택하여 Snake_Bite 스킬을 사용하는 클래스
/// </summary>
public class Snake_Bite : BaseSkill
{
    public Snake_Bite()
    {
        
    }
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        //스킬이 명중하면 앞으로 1열 이동
        if (SkillResult.IsHit(0))
        {
            BattleManager.GetInstance.MoveCharacter(SkillOwner, 1);
        }
        
    }
}
