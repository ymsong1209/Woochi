public class MC_ChangeLocation : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        SkillOwner.onPlayAnimation(AnimationType.Skill4);
    }
}
