using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WoochiRecoveryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recoveryText;
    [SerializeField] private Image recoverySorceryPoint;
    [SerializeField] private Image recoverySorceryPointBackground;
  

    public void Initialize()
    {
        gameObject.SetActive(false);
       
    }
    
    public void Activate()
    {
        gameObject.SetActive(true);
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter)
        {
            Debug.LogError("우치가 아님");
            return;
        }
        string description = "우치의 도력을 " + mainCharacter.SorceryRecoveryPoints+ "% 회복합니다.";
        recoveryText.text = description;
        float currentSorceryPointRate = (float)mainCharacter.SorceryPoints / (float)mainCharacter.MaxSorceryPoints;
        float recoveryPointsRate = mainCharacter.SorceryRecoveryPoints / 100;//*(float)mainCharacter.MaxSorceryPoints;
        float result = Mathf.Clamp(currentSorceryPointRate + recoveryPointsRate, 0, 1);
        recoverySorceryPoint.fillAmount = currentSorceryPointRate;
        recoverySorceryPointBackground.fillAmount = result;
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter)
        {
            Debug.LogError("우치가 아님");
            return;
        }
        recoverySorceryPoint.fillAmount = (float)mainCharacter.SorceryPoints / (float)mainCharacter.MaxSorceryPoints;
        recoverySorceryPointBackground.fillAmount = (float)mainCharacter.SorceryPoints / (float)mainCharacter.MaxSorceryPoints;
    }
}
