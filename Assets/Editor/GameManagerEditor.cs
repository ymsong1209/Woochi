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

        #region 데이터 삭제 버튼
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIContent buttonContent = new GUIContent("데이터 삭제", "지금까지의 데이터를 삭제하고 새로 시작하려면 누르세요");
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
