using System.Collections.Generic;
using UnityEngine;

public class WoochiButtonList : MonoBehaviour
{
    [SerializeField] private List<WoochiActionButton> buttonList;
    private WoochiActionButton selectedBtn;     // 선택한 버튼이 무엇인지

    public void Activate(bool isEnable)
    {
        gameObject.SetActive(true);
        InitializeAllButtons(isEnable);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        DeactivateAllButtons();
    }
    
    public void InitializeAllButtons(bool isEnable)
    {
        selectedBtn = null;

        foreach (var button in buttonList)
        {
            button.Initialize(isEnable);
        }
    }
    
    public void DeactivateAllButtons()
    {
        selectedBtn = null;

        foreach (var button in buttonList)
        {
            button.Deactivate();
            button.SetAlpha(0);
        }
    }

    public void SelectButton(WoochiActionButton button)
    {
        if (selectedBtn == button) return;
        selectedBtn = button;

        // 우치 행동 중 다른 버튼 누르면 콜라이더, 화살표 비활성화
        BattleManager.GetInstance.RemoveSelectedSkill();
        BattleManager.GetInstance.DisableColliderArrow();

        ActivateButton(button);
    }
    
    public void ActivateButton(WoochiActionButton button)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i] == button)
            {
                buttonList[i].Activate();
            }
            else
            {
                buttonList[i].Deactivate();
            }
        }
    }
}
