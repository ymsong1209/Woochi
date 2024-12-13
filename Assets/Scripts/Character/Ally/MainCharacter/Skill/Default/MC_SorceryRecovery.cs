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

        mainCharacter.UpdateSorceryPoints(mainCharacter.SorceryRecoveryPoints, true);
        SkillOwner.onPlayAnimation?.Invoke(AnimationType.Skill3);
        GameManager.GetInstance.soundManager.PlaySFX("Power_Up");
    }
}
