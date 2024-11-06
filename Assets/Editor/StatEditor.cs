using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StatValue))]
public class StatEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty labelProp = property.FindPropertyRelative("type");
        SerializedProperty valueProp = property.FindPropertyRelative("value");

        // 레이블과 값 입력 필드 만들기
        float halfWidth = position.width / 2;
        Rect labelRect = new Rect(position.x, position.y, halfWidth, position.height);
        Rect valueRect = new Rect(position.x + halfWidth, position.y, halfWidth, position.height);

        // GUI 표시
        EditorGUI.PropertyField(labelRect, labelProp, GUIContent.none);
        EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
    }
}
