using UnityEngine;

public class WoochiActionList : MonoBehaviour
{
    [SerializeField] private WoochiButtonList buttonList;
    [SerializeField] private WoochiSkillSelectionUI skillList;
    [SerializeField] private WoochiRecoveryUI recoveryUI;
    void Start()
    {
        buttonList.gameObject.SetActive(false);
        skillList.gameObject.SetActive(false);
        recoveryUI.gameObject.SetActive(false);
        BattleManager.GetInstance.ShowCharacterUI += ShowUI;
    }

    private void ShowUI(BaseCharacter character, bool isTurn = false)
    {
        buttonList.InitButtonList();

        if (character.IsAlly)
        {
            if (character.IsMainCharacter)
            {
                buttonList.Activate(isTurn);
            }
            else
            {
                buttonList.Deactivate();
            }
        }
        else
        {
            buttonList.InitializeAllButtons(false);
        }
    }
}
