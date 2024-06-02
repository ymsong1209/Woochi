using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiActionList : MonoBehaviour
{
    [SerializeField] private WoochiButtonList buttonList;

    [SerializeField] private WoochiSkillSelectionUI skillList;

    [SerializeField] private WoochiRecoveryUI recoveryUI;
    // Start is called before the first frame update
    void Start()
    {
        buttonList.gameObject.SetActive(false);
        skillList.gameObject.SetActive(false);
        recoveryUI.gameObject.SetActive(false);
        BattleManager.GetInstance.OnCharacterTurnStart += ShowUI;
    }

    private void ShowUI(BaseCharacter _character, bool isEnable = false)
    {
        if (_character.IsMainCharacter)
        {
            buttonList.Activate();
        }
        else
        {
            buttonList.gameObject.SetActive(false);
        }
    }
    
    public void Reset()
    {
        buttonList.Deactivate();
        skillList.Deactivate();
        recoveryUI.Deactivate();
    }
}
