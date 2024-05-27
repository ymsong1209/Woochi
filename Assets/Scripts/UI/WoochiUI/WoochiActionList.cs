using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiActionList : MonoBehaviour
{
    [SerializeField] private WoochiButtonList buttonList;

    [SerializeField] private WoochiSkillList skillList;
    // Start is called before the first frame update
    void Start()
    {
        buttonList.gameObject.SetActive(false);
        skillList.gameObject.SetActive(false);
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
    
    public void ShowSkillList()
    {
        skillList.Activate();
        //TODO : 다른 UI 비활성화
        //ChangeLocationUI.DeActivate();
        //PartyChangeUI.Deactivate(); 등등..
    }
}
