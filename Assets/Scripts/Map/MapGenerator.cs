using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapGenerator
{
    private static MapConfig config;

    // 레이어 간 y축으로 얼마나 떨어져 있는지
    private static List<float> layerDistances;

    // 1차적으로 생성할 모든 노드들(후에 조건에 따라 일부 노드들을 제거할 수 있음)
    private static readonly List<List<Node>> nodes = new List<List<Node>>();

    public static Map GetMap(MapConfig conf)
    {
        if (conf == null)
            return null;

        config = conf;
        nodes.Clear();

        GenerateLayerDistances();

        for (int i = 0; i < conf.layers.Count; i++)
            PlaceLayer(i);

        List<List<Vector2Int>> paths = GeneratePaths();

        RandomizeNodePositions();

        SetUpConnections(paths);

        RemoveCrossConnections();

        // 연결이 있는 노드들만 남김
        List<Node> nodesList = nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

        return new Map(conf.name, nodesList, new List<Vector2Int>());
    }

    private static void GenerateLayerDistances()
    {
        layerDistances = new List<float>();
        foreach (MapLayer layer in config.layers)
            layerDistances.Add(layer.distanceFromPreviousLayer.GetValue());
    }

    private static float GetDistanceToLayer(int layerIndex)
    {
        if (layerIndex < 0 || layerIndex > layerDistances.Count) return 0f;

        return layerDistances.Take(layerIndex + 1).Sum();
    }

    private static void PlaceLayer(int layerIndex)
    {
        MapLayer layer = config.layers[layerIndex];
        List<Node> nodesOnThisLayer = new List<Node>();

        // offset of this layer to make all the nodes centered:
        float offset = layer.nodesApartDistance * config.GridWidth / 2f;

        for (int i = 0; i < config.GridWidth; i++)
        {
            NodeType nodeType = layer.isRandomNode ? config.GetNodeType() : layer.nodeType;
            Node node = new Node(nodeType, new Vector2Int(i, layerIndex))
            {
                position = new Vector2(-offset + i * layer.nodesApartDistance, GetDistanceToLayer(layerIndex)),
                abnormalID = nodeType == NodeType.Strange ? 0 : config.GetAbnormal(),
                enemyIDs = layer.isRandomEnemy ? GetEnemyID(nodeType) : layer.enemyIDs
            };

            nodesOnThisLayer.Add(node);
        }

        nodes.Add(nodesOnThisLayer);
    }
    
    /// <summary>
    /// 해당 노드에 나타날 적의 ID들을 설정함
    /// 게임오브젝트를 설정하면 직렬화가 안되어 저장이 안됨
    /// </summary>
    private static int[] GetEnemyID(NodeType _nodeType)
    {
        switch(_nodeType)
        {
            case NodeType.Normal:
                return config.GetNormalEnemy();
            case NodeType.Elite:
                return config.GetEliteEnemy();
            case NodeType.Strange:
                return config.GetNormalEnemy();
            case NodeType.Boss:
                break;
        }

        return null;
    }

    private static void RandomizeNodePositions()
    {
        for (int index = 0; index < nodes.Count; index++)
        {
            List<Node> list = nodes[index];
            MapLayer layer = config.layers[index];
            float distToNextLayer = index + 1 >= layerDistances.Count
                ? 0f
                : layerDistances[index + 1];
            float distToPreviousLayer = layerDistances[index];

            foreach (Node node in list)
            {
                float xRnd = Random.Range(-0.5f, 0.5f);
                float yRnd = Random.Range(-0.3f, 0.3f);

                float x = xRnd * layer.nodesApartDistance;
                float y = yRnd < 0 ? distToPreviousLayer * yRnd : distToNextLayer * yRnd;

                node.position += new Vector2(x, y) * layer.randomizePosition;
            }
        }
    }

    private static void SetUpConnections(List<List<Vector2Int>> paths)
    {
        foreach (List<Vector2Int> path in paths)
        {
            for (int i = 0; i < path.Count - 1; ++i)
            {
                Node node = GetNode(path[i]);
                Node nextNode = GetNode(path[i + 1]);
                node.AddOutgoing(nextNode.point);
                nextNode.AddIncoming(node.point);
            }
        }
    }

    private static void RemoveCrossConnections()
    {
        for (int i = 0; i < config.GridWidth - 1; ++i)
            for (int j = 0; j < config.layers.Count - 1; ++j)
            {
                Node node = GetNode(new Vector2Int(i, j));
                if (node == null || node.HasNoConnections()) continue;
                Node right = GetNode(new Vector2Int(i + 1, j));
                if (right == null || right.HasNoConnections()) continue;
                Node top = GetNode(new Vector2Int(i, j + 1));
                if (top == null || top.HasNoConnections()) continue;
                Node topRight = GetNode(new Vector2Int(i + 1, j + 1));
                if (topRight == null || topRight.HasNoConnections()) continue;

                // Debug.Log("Inspecting node for connections: " + node.point);
                if (!node.outgoing.Any(element => element.Equals(topRight.point))) continue;
                if (!right.outgoing.Any(element => element.Equals(top.point))) continue;

                // Debug.Log("Found a cross node: " + node.point);

                // we managed to find a cross node:
                // 1) add direct connections:
                node.AddOutgoing(top.point);
                top.AddIncoming(node.point);

                right.AddOutgoing(topRight.point);
                topRight.AddIncoming(right.point);

                float rnd = Random.Range(0f, 1f);
                if (rnd < 0.2f)
                {
                    // remove both cross connections:
                    // a) 
                    node.RemoveOutgoing(topRight.point);
                    topRight.RemoveIncoming(node.point);
                    // b) 
                    right.RemoveOutgoing(top.point);
                    top.RemoveIncoming(right.point);
                }
                else if (rnd < 0.6f)
                {
                    // a) 
                    node.RemoveOutgoing(topRight.point);
                    topRight.RemoveIncoming(node.point);
                }
                else
                {
                    // b) 
                    right.RemoveOutgoing(top.point);
                    top.RemoveIncoming(right.point);
                }
            }
    }

    private static Node GetNode(Vector2Int p)
    {
        if (p.y >= nodes.Count) return null;
        if (p.x >= nodes[p.y].Count) return null;

        return nodes[p.y][p.x];
    }

    /// <summary>
    /// 맵의 마지막 노드를 반환
    /// </summary>
    private static Vector2Int GetFinalNode()
    {
        int y = config.layers.Count - 1;

        // GridWidth가 홀수일 때는 가운데에 노드가 있음
        if (config.GridWidth % 2 == 1)
            return new Vector2Int(config.GridWidth / 2, y);

        return Random.Range(0, 2) == 0
            ? new Vector2Int(config.GridWidth / 2, y)
            : new Vector2Int(config.GridWidth / 2 - 1, y);
    }

    private static List<List<Vector2Int>> GeneratePaths()
    {
        Vector2Int finalNode = GetFinalNode();
        var paths = new List<List<Vector2Int>>();
        int numOfStartingNodes = config.numOfStartingNodes.GetValue();
        int numOfPreBossNodes = config.numOfPreBossNodes.GetValue();

        List<int> candidateXs = new List<int>();
        for (int i = 0; i < config.GridWidth; i++)
            candidateXs.Add(i);

        candidateXs.Shuffle();
        IEnumerable<int> startingXs = candidateXs.Take(numOfStartingNodes);
        List<Vector2Int> startingPoints = (from x in startingXs select new Vector2Int(x, 0)).ToList();

        candidateXs.Shuffle();
        IEnumerable<int> preBossXs = candidateXs.Take(numOfPreBossNodes);
        List<Vector2Int> preBossPoints = (from x in preBossXs select new Vector2Int(x, finalNode.y - 1)).ToList();

        int numOfPaths = Mathf.Max(numOfStartingNodes, numOfPreBossNodes) + Mathf.Max(0, config.extraPaths);
        for (int i = 0; i < numOfPaths; ++i)
        {
            Vector2Int startNode = startingPoints[i % numOfStartingNodes];
            Vector2Int endNode = preBossPoints[i % numOfPreBossNodes];
            List<Vector2Int> path = Path(startNode, endNode);
            path.Add(finalNode);
            paths.Add(path);
        }

        return paths;
    }

    // Generates a random path bottom up.
    private static List<Vector2Int> Path(Vector2Int fromPoint, Vector2Int toPoint)
    {
        int toRow = toPoint.y;
        int toCol = toPoint.x;

        int lastNodeCol = fromPoint.x;

        List<Vector2Int> path = new List<Vector2Int> { fromPoint };
        List<int> candidateCols = new List<int>();
        for (int row = 1; row < toRow; ++row)
        {
            candidateCols.Clear();

            int verticalDistance = toRow - row;
            int horizontalDistance;

            int forwardCol = lastNodeCol;
            horizontalDistance = Mathf.Abs(toCol - forwardCol);
            if (horizontalDistance <= verticalDistance)
                candidateCols.Add(lastNodeCol);

            int leftCol = lastNodeCol - 1;
            horizontalDistance = Mathf.Abs(toCol - leftCol);
            if (leftCol >= 0 && horizontalDistance <= verticalDistance)
                candidateCols.Add(leftCol);

            int rightCol = lastNodeCol + 1;
            horizontalDistance = Mathf.Abs(toCol - rightCol);
            if (rightCol < config.GridWidth && horizontalDistance <= verticalDistance)
                candidateCols.Add(rightCol);

            int randomCandidateIndex = Random.Range(0, candidateCols.Count);
            int candidateCol = candidateCols[randomCandidateIndex];
            Vector2Int nextPoint = new Vector2Int(candidateCol, row);

            path.Add(nextPoint);

            lastNodeCol = candidateCol;
        }

        path.Add(toPoint);

        return path;
    }
}