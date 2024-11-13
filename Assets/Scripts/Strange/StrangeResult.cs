using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/None")]
public class StrangeResult : ScriptableObject
{
    public Sprite sprite;   // 결과 이미지
    [TextArea(3, 10)]
    public string text;     // 결과 텍스트
    [TextArea(3, 10)]
    public string effect;   // 결과 효과

    public virtual void ApplyEffect()
    {

    }
}
