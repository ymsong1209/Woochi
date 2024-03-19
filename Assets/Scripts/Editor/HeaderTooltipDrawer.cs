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
        // Header ����
        Rect headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(headerRect, HeaderTooltip.header, EditorStyles.boldLabel);

        // Tooltip ����
        // ���콺�� ��� ���� ���� �ִ��� üũ
        if (position.Contains(Event.current.mousePosition))
        {
            TooltipPopupWindow.ShowTooltip(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), HeaderTooltip.tooltip);
        }
    }
}
