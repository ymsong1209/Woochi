using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ������ ���� � ����� ���̴��� ��Ÿ���� ��ũ���ͺ� ������Ʈ
/// </summary>
[CreateAssetMenu(fileName = "NodeBlueprint_", menuName = "Scriptable Objects/Map/NodeBlueprint")]
public class NodeBlueprint : ScriptableObject
{
    public Sprite icon;
    public NodeType type;
}
