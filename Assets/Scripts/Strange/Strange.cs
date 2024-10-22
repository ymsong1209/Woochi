using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/Strange")]
public class Strange : ScriptableObject
{
    [Header("Situation")]
    public Sprite situationSprite;     // ��Ȳ ���� �̹���
    [TextArea(3, 10)]
    public string situationText;       // ��Ȳ ���� �ؽ�Ʈ

    [Header("Choice")]
    public Choice[] choices;  // ���� �⿬���� ������ �� �ִ� ��������

    public StrangeResult Select(int choiceIndex)
    {
        if(choiceIndex < 0 || choiceIndex >= choices.Length)
        {
            Debug.LogError("Invalid choice index");
            return null;
        }

        Choice choice = choices[choiceIndex];
        StrangeResult result = choice.results.Get();
        result.ApplyEffect();

        return result;
    }
}

[System.Serializable]
public class Choice
{
    public string text;     // ������ �ؽ�Ʈ
    public RandomList<StrangeResult> results;   // ������ �����
}
