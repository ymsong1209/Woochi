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
       BattleManager.GetInstance.CharacterSelected(mainCharacter);
       BattleManager.GetInstance.ExecuteSelectedSkill(mainCharacter);
    }
}
