using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ITooltipiable tooltipable;

    void Awake()
    {
        tooltipable = GetComponent<ITooltipiable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipable != null)
        { 
            tooltipable.ShowTooltip();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipable != null)
        {
            tooltipable.HideTooltip();
        }
    }
}
