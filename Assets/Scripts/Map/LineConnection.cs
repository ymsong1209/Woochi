using UnityEngine;
using UnityEngine.UI.Extensions;

[System.Serializable]
public class LineConnection
{
    public UILineRenderer uilr;
    public MapNode from;
    public MapNode to;

    public LineConnection(UILineRenderer uilr, MapNode from, MapNode to)
    {
        this.uilr = uilr;
        this.from = from;
        this.to = to;
    }

    public void SetColor(Color color)
    {
        if (uilr != null) uilr.color = color;
    }
}
