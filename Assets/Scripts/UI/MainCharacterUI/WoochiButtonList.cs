using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiButtonList : MonoBehaviour
{
    [SerializeField] private List<WoochiActionButton> buttonList;
    private WoochiActionButton selectedBtn;     // ������ ��ư�� ��������

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
        }
    }

    public void SelectButton(WoochiActionButton button)
    {
        if (selectedBtn == button) return;
        selectedBtn = button;

        // ��ġ �ൿ �� �ٸ� ��ư ������ �ݶ��̴�, ȭ��ǥ ��Ȱ��ȭ
        BattleManager.GetInstance.DisableColliderArrow();

        ActivateButton(button);
        HighlightButton(button);
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
    
    
    public void HighlightButton(WoochiActionButton button)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].Interactable(true);

            if (buttonList[i] == button)
            {
                buttonList[i].Highlight();
            }
            else
            {
                buttonList[i].DeHighlight();
            }
        }
    }
}
