using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/None")]
public class StrangeResult : ScriptableObject
{
    public Sprite sprite;   // ��� �̹���
    [TextArea(3, 10)]
    public string text;     // ��� �ؽ�Ʈ
    [TextArea(3, 10)]
    public string effect;   // ��� ȿ��

    public virtual void ApplyEffect()
    {

    }
}
