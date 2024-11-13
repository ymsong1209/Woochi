using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        GUILayout.Label("Custom Inspector", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GameManager gameManager = GameManager.GetInstance;

        #region ������ ���� ��ư
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIContent buttonContent = new GUIContent("������ ����", "���ݱ����� �����͸� �����ϰ� ���� �����Ϸ��� ��������");
            if (GUILayout.Button(buttonContent, GUILayout.Width(100), GUILayout.Height(50)))
            {
                gameManager.DeleteData();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}
