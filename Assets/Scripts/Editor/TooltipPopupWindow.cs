using UnityEditor;
using UnityEngine;

public class TooltipPopupWindow : EditorWindow
{
    private static string message = "";
    private static Vector2 windowSize = new Vector2(200, 100);

    public static void ShowTooltip(Vector2 position, string tooltipMessage)
    {
        var window = GetWindow<TooltipPopupWindow>();
        window.position = new Rect(position.x, position.y, windowSize.x, windowSize.y);
        message = tooltipMessage;
        window.ShowPopup();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField(message, EditorStyles.wordWrappedLabel);
    }

    private void OnLostFocus()
    {
        Close(); // ÆË¾÷ Ã¢ÀÌ Æ÷Ä¿½º¸¦ ÀÒÀ¸¸é ÀÚµ¿À¸·Î ´ÝÈû
    }
}
