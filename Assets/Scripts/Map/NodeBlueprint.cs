using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 노드의 종류에 따라 어떤 모습을 보이는지 나타내는 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName = "NodeBlueprint_", menuName = "Scriptable Objects/Map/NodeBlueprint")]
public class NodeBlueprint : ScriptableObject
{
    public Sprite icon;
    public NodeType type;
}
