using UnityEngine;

public class MC_SorceryRecovery : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        MainCharacter mainCharacter = SkillOwner as MainCharacter;
        if (mainCharacter is null)
        {
            Debug.LogError("우치가 아님");
            return;
        }

        float temp = mainCharacter.SorceryPoints;
        mainCharacter.UpdateSorceryPoints(mainCharacter.MaxSorceryPoints, true);
        Logger.BattleLog($"\"{SkillOwner.Name}\"({SkillOwner.RowOrder + 1}열)의 도력이 {temp}에서 {mainCharacter.SorceryPoints}로 회복", "우치 스킬[도력 회복]");
        SkillOwner.onPlayAnimation?.Invoke(AnimationType.Skill3);
        GameManager.GetInstance.soundManager.PlaySFX("Power_Up");
    }
}
