using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/Strange")]
public class Strange : ScriptableObject
{
    [Header("Situation")]
    public Sprite situationSprite;     // 상황 설명 이미지
    [TextArea(3, 10)]
    public string situationText;       // 상황 설명 텍스트

    [Header("Choice")]
    public Choice[] choices;  // 현재 기연에서 선택할 수 있는 선택지들

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
    public string text;    // 선택지 텍스트
    public RandomList<StrangeResult> results;   // 선택지 결과들
}
