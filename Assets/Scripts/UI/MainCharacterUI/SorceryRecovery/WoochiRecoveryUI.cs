using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WoochiRecoveryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recoveryText;
    [SerializeField] private Button recoveryConfirmButton;
    
    public void Activate()
    {
        gameObject.SetActive(true);
        recoveryConfirmButton.interactable = true;
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter)
        {
            Debug.LogError("우치가 차례가 아닌데 도력 UI 활성화됨");
            return;
        }
        
        string description = $"우치의 도력을 회복합니다";
        recoveryText.text = description;
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
        UIManager.GetInstance.sorceryGuageUI.Restore();
    }
}
