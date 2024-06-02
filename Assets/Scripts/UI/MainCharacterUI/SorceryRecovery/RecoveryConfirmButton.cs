using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryConfirmButton : MonoBehaviour
{
    
    
    public void RecoveryConfirm()
    { 
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if(!mainCharacter)
        {
            Debug.LogError("우치가 아님");
            return;
        }
        MC_SorceryRecovery recoveryskill = mainCharacter.SorceryRecoverySkill; 
       BattleManager.GetInstance.SkillSelected(recoveryskill);
       BattleManager.GetInstance.ExecuteSelectedSkill(BattleManager.GetInstance.currentCharacter);
    }
}
