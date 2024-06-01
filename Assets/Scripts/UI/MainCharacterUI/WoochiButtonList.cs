using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiButtonList : MonoBehaviour
{
    [SerializeField] private List<WoochiActionButton> buttonList;

    public void Activate()
    {
        gameObject.SetActive(true);
        InitializeAllButtons();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        DeactivateAllButtons();
    }
    
    public void InitializeAllButtons()
    {
        foreach (var button in buttonList)
        {
            button.Initialize();
        }
    }
    
    public void DeactivateAllButtons()
    {
        foreach (var button in buttonList)
        {
            button.Deactivate();
        }
    }

    public void SelectButton(WoochiActionButton button)
    {
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
