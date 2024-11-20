using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ITooltipiable tooltipable;

    private void Awake()
    {
        tooltipable = GetComponent<ITooltipiable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipable?.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipable?.HideTooltip();
    }
}
