using UnityEngine;

/// <summary>
/// ����� ������ ���� � ����� ���̴��� ��Ÿ���� ��ũ���ͺ� ������Ʈ
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Map/NodeBlueprint")]
public class NodeBlueprint : ScriptableObject
{
    public Sprite sprite;
    public NodeType nodeType;
}
