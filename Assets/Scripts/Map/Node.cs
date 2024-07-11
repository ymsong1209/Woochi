using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Vector2Int       point;                              // 노드의 위치 정보(실제 position이 아님)
    public List<Vector2Int> incoming = new List<Vector2Int>();  // 이 노드로 들어오는 노드들
    public List<Vector2Int> outgoing = new List<Vector2Int>();  // 이 노드에서 나가는 노드들
    [JsonConverter(typeof(StringEnumConverter))]
    public NodeType    nodeType;                                
    public Vector2     position;                                // 노드가 Canvas에서 어디에 위치하는지

    public int   abnormalID;                                    // 이 노드에서 발생하는 이상 ID
    public int[] enemyIDs;                                      // 이 노드에서 등장하는 적들의 ID  

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
    /// 연결되지 않은 노드인지
    /// </summary>
    public bool HasNoConnections()
    {
        return incoming.Count == 0 && outgoing.Count == 0;
    }
}
