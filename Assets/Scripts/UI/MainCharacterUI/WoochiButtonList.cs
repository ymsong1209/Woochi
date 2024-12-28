using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoochiButtonList : MonoBehaviour
{
    [SerializeField] private List<WoochiActionButton> buttonList;
    [SerializeField] private ToggleGroup toggleGroup;
    private WoochiActionButton selectedBtn;     // 선택한 버튼이 무엇인지

    public void Activate(bool isEnable)
    {
        gameObject.SetActive(true);
        InitializeAllButtons(isEnable);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    
    public void InitializeAllButtons(bool isEnable)
    {
        foreach (var button in buttonList)
        {
            button.Initialize(isEnable);
        }
    }

    public void SelectButton(WoochiActionButton button)
    {
        toggleGroup.allowSwitchOff = false;
        
        if (button.IsOn)
        {
            if (button == selectedBtn) return;
            button.Activate();
            selectedBtn = button;

            ScenarioManager.GetInstance.NextPlot(PlotEvent.Click);
        }
        else
        {
            button.Deactivate();
        }
    }
    
    public void InitButtonList()
    {
        toggleGroup.allowSwitchOff = true;
        toggleGroup.SetAllTogglesOff();
        selectedBtn = null;
    }
}
