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
        BattleManager.GetInstance.OnCharacterAttacked += ShowUI;
    }

    private void ShowUI(BaseCharacter _character, bool isEnable = false)
    {
        // 우치인 경우 우치 UI 활성화
        // 우치가 스킬을 사용한 후에는 Reset함수 호출 후, isEnable이 false로 들어옴
        if (_character.IsMainCharacter)
        {
            buttonList.Activate(isEnable);
        }
        // 우치가 아니고 아군의 차례가 시작 or 아군이 피격된 경우 우치 UI 전체 비활성화
        else if(!_character.IsMainCharacter && _character.IsAlly)
        {
            buttonList.Deactivate();
        }
        // 적군이 우치 공격한 경우에는 우치 기술 버튼만 비활성화
        else if(!_character.IsAlly)
        {
            buttonList.DeactivateAllButtons();
        }
    }
   
    //우치가 스킬 사용한 후, Reset함수로 우치 UI 전체 비활성화
    //이후, oncharacterattacked에서 isenable이 false로 들어옴.
    public void Reset()
    {
        //buttonList.Deactivate();
    }
}
