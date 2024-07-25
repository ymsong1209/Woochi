using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    public List<Node> nodes;        // ���� ����
    public List<Vector2Int> path;   // �÷��̾ �̵��� ���
    public string configName;       // �� Map�� ���� �� ����� config�� �̸� => Json ���� ���� string

    [JsonConstructor]
    public Map(string configName, List<Node> nodes, List<Vector2Int> path)
    {
        this.configName = configName;
        this.nodes = nodes;
        this.path = path;
    }

    public Map(Map _map)
    {
        configName = _map.configName;
        nodes = new List<Node>(_map.nodes);
        path = new List<Vector2Int>(_map.path);
    }

    public Node GetBossNode()
    {
        return nodes.FirstOrDefault(n => n.nodeType == NodeType.Boss);
    }

    /// <summary>
    /// ���� ���� layer�� ������ layer ������ y�� �Ÿ��� ��ȯ
    /// </summary>
    public float DistanceBetweenFirstAndLastLayers()
    {
        Node bossNode = GetBossNode();
        Node firstLayerNode = nodes.FirstOrDefault(n => n.point.y == 0);

        if (bossNode == null || firstLayerNode == null)
            return 0f;

        return bossNode.position.y - firstLayerNode.position.y;
    }

    /// <summary>
    /// point�� ��ġ�� ��ġ�ϴ� ��带 ��ȯ
    /// </summary>
    public Node GetNode(Vector2Int point)
    {
        return nodes.FirstOrDefault(n => n.point.Equals(point));
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }
}
