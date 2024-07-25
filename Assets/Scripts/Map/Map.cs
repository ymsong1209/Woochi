using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    public List<Node> nodes;        // 맵의 노드들
    public List<Vector2Int> path;   // 플레이어가 이동한 경로
    public string configName;       // 이 Map을 만들 때 사용한 config의 이름 => Json 저장 위해 string

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
    /// 맵의 시작 layer와 마지막 layer 사이의 y축 거리를 반환
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
    /// point와 위치가 일치하는 노드를 반환
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
