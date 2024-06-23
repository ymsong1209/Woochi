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
        float result = Mathf.Clamp(mainCharacter.SorceryPoints + mainCharacter.SorceryRecoveryPoints, 0,
            mainCharacter.MaxSorceryPoints);
        mainCharacter.SorceryPoints = (int)result;
        
        SkillOwner.onPlayAnimation?.Invoke(AnimationType.Skill3);
    }
}
