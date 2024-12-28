public class MC_ChangeLocation : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        int temp = SkillOwner.RowOrder;
        int temp2 = _Opponent.RowOrder;
        BattleManager.GetInstance.ChangeCharacterLocation();
        Logger.BattleLog($"\"{SkillOwner.Name}\"({temp + 1}열)이(가) \"{_Opponent.Name}\"({temp2 + 1}열)이랑 자리바꿈 시전\n" +
                                $"\"{SkillOwner.Name}\"({SkillOwner.RowOrder + 1}열), \"{_Opponent.Name}\"({_Opponent.RowOrder + 1}열)", "우치 스킬[자리바꿈]");
        SkillOwner.onPlayAnimation(AnimationType.Skill4);
        GameManager.GetInstance.soundManager.PlaySFX("Swap");
    }
}
