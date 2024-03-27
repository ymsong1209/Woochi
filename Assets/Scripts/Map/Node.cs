using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// ToDo. 예제라 여기에 선언, 확정되면 옮기기
/// <summary>
/// 방의 종류
/// </summary>
public enum NodeType
{
    Dungeon,    // 던전
    Strange,    // 기연 -> 행운, 불운으로 다시 나뉨
    Camp,       // 야영
    Tavern,     // 주막
    Boss,        // 보스
    END
}

public class Node
{
    public readonly Point       point;
    public readonly List<Point> incoming = new List<Point>();  // 이 노드로 들어오는 노드들
    public readonly List<Point> outgoing = new List<Point>();  // 이 노드에서 나가는 노드들

    public readonly NodeType    type;
    public readonly string      blueprintName;
    public          Vector2     position;

    public Node(NodeType _type, string _blueprintName, Point _point)
    {
        point = _point;
        type = _type;
        blueprintName = _blueprintName;
    }

    public void AddIncoming(Point _point)
    {
        if(incoming.Any(element => element.Equals(_point)))
            return;

        incoming.Add(_point);
    }

    public void AddOutgoing(Point _point)
    {
        if(outgoing.Any(element => element.Equals(_point)))
            return;

        outgoing.Add(_point);
    }

    public void RemoveIncoming(Point _point)
    {
        incoming.RemoveAll(element => element.Equals(_point));
    }

    public void RemoveOutgoing(Point _point)
    {
        outgoing.RemoveAll(element => element.Equals(_point));
    }

    /// <summary>
    /// 연결되지 않은 노드인지
    /// </summary>
    /// <returns></returns>
    public bool IsUnconnect()
    {
        return incoming.Count == 0 && outgoing.Count == 0;
    }
}
