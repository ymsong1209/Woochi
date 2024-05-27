using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiActionList : MonoBehaviour
{
    [SerializeField] private GameObject buttonList;
    // Start is called before the first frame update
    void Start()
    {
        buttonList.SetActive(false);
        BattleManager.GetInstance.OnCharacterTurnStart += ShowUI;
    }

    private void ShowUI(BaseCharacter _character, bool isEnable = false)
    {
        if (_character.IsMainCharacter) buttonList.SetActive(true);
        else buttonList.SetActive(false);
    }
}
