using UnityEngine;
using TMPro;

public class WoochiRecoveryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recoveryText;
    
    public void Activate()
    {
        gameObject.SetActive(true);
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter)
        {
            Debug.LogError("우치가 차례가 아닌데 도력 UI 활성화됨");
            return;
        }

        int recovery = Mathf.CeilToInt(mainCharacter.SorceryRecoveryPoints * mainCharacter.MaxSorceryPoints);
        int ableRecovery = Mathf.Clamp(mainCharacter.MaxSorceryPoints - mainCharacter.SorceryPoints, 0, recovery);
        string description = $"우치의 도력을 {mainCharacter.SorceryRecoveryPoints * 100}%({ableRecovery}) 회복합니다";
        recoveryText.text = description;
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
        UIManager.GetInstance.sorceryGuageUI.Restore();
    }
}
