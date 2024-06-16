public class MC_Summon : BaseSkill
{
    public BaseCharacter willSummon;    // 소환될 캐릭터
    public bool isSummon = false;       // 소환할 것인지 소환 해제할 것인지

    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        // 소환, 소환 해제 시 특별한 애니메이션 있으면 여기서 처리할 것
        if(isSummon)
        {
            BattleManager.GetInstance.Summon(willSummon, _Opponent);
            willSummon.onPlayAnimation?.Invoke(AnimationType.Idle);
        }
        else
        {

        }


        SkillOwner.onPlayAnimation?.Invoke(AnimationType.Skill2);
    }
}
