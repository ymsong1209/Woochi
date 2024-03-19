using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HeaderTooltipAttribute))]
public class HeaderTooltipDrawer : DecoratorDrawer
{
    HeaderTooltipAttribute HeaderTooltip => attribute as HeaderTooltipAttribute;

    public override float GetHeight()
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position)
    {
        // Header 영역
        Rect headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(headerRect, HeaderTooltip.header, EditorStyles.boldLabel);

        // Tooltip 영역
        // 마우스가 헤더 영역 위에 있는지 체크
        if (position.Contains(Event.current.mousePosition))
        {
            TooltipPopupWindow.ShowTooltip(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), HeaderTooltip.tooltip);
        }
    }
}
