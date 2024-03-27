using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// ToDo. ������ ���⿡ ����, Ȯ���Ǹ� �ű��
/// <summary>
/// ���� ����
/// </summary>
public enum NodeType
{
    Dungeon,    // ����
    Strange,    // �⿬ -> ���, �ҿ����� �ٽ� ����
    Camp,       // �߿�
    Tavern,     // �ָ�
    Boss,        // ����
    END
}

public class Node
{
    public readonly Point       point;
    public readonly List<Point> incoming = new List<Point>();  // �� ���� ������ ����
    public readonly List<Point> outgoing = new List<Point>();  // �� ��忡�� ������ ����

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
    /// ������� ���� �������
    /// </summary>
    /// <returns></returns>
    public bool IsUnconnect()
    {
        return incoming.Count == 0 && outgoing.Count == 0;
    }
}
