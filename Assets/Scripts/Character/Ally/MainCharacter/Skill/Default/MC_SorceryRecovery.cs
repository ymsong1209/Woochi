using System.Collections;
using System.Collections.Generic;
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

        int finalpoint = mainCharacter.SorceryPoints +
                         (int)(mainCharacter.SorceryRecoveryPoints * mainCharacter.MaxSorceryPoints / 100.0f);
        float result = Mathf.Clamp(finalpoint, 0, mainCharacter.MaxSorceryPoints);
        mainCharacter.SorceryPoints = (int)result;
        
        SkillOwner.onPlayAnimation?.Invoke(AnimationType.Skill3);
    }
}
