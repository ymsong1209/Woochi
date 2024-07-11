using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Vector2Int       point;                              // ����� ��ġ ����(���� position�� �ƴ�)
    public List<Vector2Int> incoming = new List<Vector2Int>();  // �� ���� ������ ����
    public List<Vector2Int> outgoing = new List<Vector2Int>();  // �� ��忡�� ������ ����
    [JsonConverter(typeof(StringEnumConverter))]
    public NodeType    nodeType;                                
    public Vector2     position;                                // ��尡 Canvas���� ��� ��ġ�ϴ���

    public int   abnormalID;                                    // �� ��忡�� �߻��ϴ� �̻� ID
    public int[] enemyIDs;                                      // �� ��忡�� �����ϴ� ������ ID  

    public Node(NodeType _type, Vector2Int _point)
    {
        nodeType = _type;
        point = _point;
    }

    public void AddIncoming(Vector2Int _point)
    {
        if(incoming.Any(element => element.Equals(_point)))
            return;

        incoming.Add(_point);
    }

    public void AddOutgoing(Vector2Int _point)
    {
        if(outgoing.Any(element => element.Equals(_point)))
            return;

        outgoing.Add(_point);
    }

    public void RemoveIncoming(Vector2Int _point)
    {
        incoming.RemoveAll(element => element.Equals(_point));
    }

    public void RemoveOutgoing(Vector2Int _point)
    {
        outgoing.RemoveAll(element => element.Equals(_point));
    }

    /// <summary>
    /// ������� ���� �������
    /// </summary>
    public bool HasNoConnections()
    {
        return incoming.Count == 0 && outgoing.Count == 0;
    }
}
