using UnityEngine;

public class HeaderTooltipAttribute : PropertyAttribute
{
    public string header;
    public string tooltip;

    public HeaderTooltipAttribute(string header, string tooltip)
    {
        this.header = header;
        this.tooltip = tooltip;
    }
}
