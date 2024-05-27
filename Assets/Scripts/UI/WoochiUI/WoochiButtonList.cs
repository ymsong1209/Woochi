using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiButtonList : MonoBehaviour
{
    [SerializeField] private List<WoochiActionButton> buttonList;

    public void Activate()
    {
        gameObject.SetActive(true);
        ActivateButton();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        DeactivateButton();
    }



    public void ActivateButton()
    {
        foreach (var button in buttonList)
        {
            button.Activate();
        }
    }
    
    public void DeactivateButton()
    {
        foreach (var button in buttonList)
        {
            button.Deactivate();
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
