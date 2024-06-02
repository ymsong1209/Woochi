using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiActionList : MonoBehaviour
{
    [SerializeField] private WoochiButtonList buttonList;

    [SerializeField] private WoochiSkillSelectionUI skillList;

    void Start()
    {
        buttonList.gameObject.SetActive(false);
        skillList.gameObject.SetActive(false);
        BattleManager.GetInstance.OnCharacterTurnStart += ShowUI;
        BattleManager.GetInstance.OnCharacterAttacked += ShowUI;
    }

    private void ShowUI(BaseCharacter _character, bool isEnable = false)
    {
        // 우치인 경우 우치 UI 활성화
        if (_character.IsMainCharacter)
        {
            buttonList.Activate(isEnable);
        }
        // 우치가 아니고 적이 아닌 경우 우치 UI 비활성화
        else if(!_character.IsMainCharacter && _character.IsAlly)
        {
            Reset();
        }
        else if(!_character.IsAlly)
        {
            // 우치 UI 선택된 스킬 해제
            buttonList.DeactivateAllButtons();
            skillList.Initialize();
        }
    }
    
    public void ShowSkillList()
    {
        skillList.Activate();
        //TODO : 다른 UI 비활성화
        //ChangeLocationUI.DeActivate();
        //PartyChangeUI.Deactivate(); 등등..
    }

    public void Reset()
    {
        buttonList.Deactivate();
        skillList.Deactivate();
    }
}
