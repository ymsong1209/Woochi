
using System.Collections.Generic;

public class MC_Summon : BaseSkill
{
    public BaseCharacter willSummon;    // 소환될 캐릭터
    public bool isSummon = false;       // 소환할 것인지 소환 해제할 것인지

    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        _Opponent.anim.DeactivateOutline();

        // 소환, 소환 해제 시 특별한 애니메이션 있으면 여기서 처리할 것
        if(isSummon)
        {
            BattleManager.GetInstance.Summon(willSummon, _Opponent);
            
            Logger.BattleLog($"\"{SkillOwner.Name}\"({SkillOwner.RowOrder + 1}열)이(가) \"{_Opponent.Name}\"소환을 시전", "우치 스킬[소환]");
            
        }
        else
        {
            Logger.BattleLog($"\"{SkillOwner.Name}\"({SkillOwner.RowOrder + 1}열)이(가) \"{_Opponent.Name}\"소환 해제를 시전", "우치 스킬[소환 해제]");
        }
        // 소환 후 아군 위치 정보 로그 출력
        var allies = BattleManager.GetInstance.Allies.formation;
        List<string> alliesInfo = new List<string>();

        for (int i = 0; i < allies.Length; i++)
        {
            BaseCharacter ally = allies[i];
            if (ally != null && !ally.IsDead)
            {
                alliesInfo.Add($"\"{ally.Name}\"({ally.RowOrder + 1}열)");
                // 크기가 2인 경우엔 현재 ally를 기록 후 다음 index는 건너뜀
                if (ally.Size == 2)
                {
                    i++;
                }
            }
        }

        Logger.BattleLog("아군 위치 : " + string.Join(" - ", alliesInfo), "위치 정보");
        SkillOwner.onPlayAnimation?.Invoke(AnimationType.Skill2);
    }
}
