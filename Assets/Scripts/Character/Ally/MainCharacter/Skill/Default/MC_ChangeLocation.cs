public class MC_ChangeLocation : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        BattleManager.GetInstance.ChangeCharacterLocation();
        SkillOwner.onPlayAnimation(AnimationType.Skill4);
        AkSoundEngine.PostEvent("Swap", gameObject);
    }
}
