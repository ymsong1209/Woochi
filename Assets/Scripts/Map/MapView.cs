using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 생성한 Map을 화면에 표시하는 클래스
/// </summary>
public class MapView : MonoBehaviour
{
    [SerializeField] private List<MapConfig> allMapConfigs;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private List<NodeBlueprint> nodeBlueprints;

    [Header("Stage Settings")]
    public Sprite background;

    [Header("Line Settings")]
    [Tooltip("Distance from the node till the line starting point")]
    public float offsetFromNodes = 1f;
    public float lineSpacing = 10f;

    [Space]
    [Tooltip("보정값")]
    public Vector2 correction = Vector2.zero;

    [Header("Colors")]
    [Tooltip("Node Visited or Attainable color")]
    public Color32 visitedColor = Color.white;
    [Tooltip("Locked node color")]
    public Color32 lockedColor = Color.gray;
    [Tooltip("Visited or available path color")]
    public Color32 lineVisitedColor = Color.white;
    [Tooltip("Unavailable path color")]
    public Color32 lineLockedColor = Color.gray;

    [Header("UI Map Settings")]
    [SerializeField] private GameObject mapObject;
    [SerializeField] private ScrollRect scrollRect;
    [Tooltip("Multiplier to compensate for larger distances in UI pixels on the canvas compared to distances in world units")]
    [SerializeField] private float unitsToPixelsMultiplier = 10f;
    [Tooltip("Padding of the first and last rows of nodes from the sides of the scroll rect")]
    [SerializeField] private float padding;
    [Tooltip("Padding of the background from the sides of the scroll rect")]
    [SerializeField] private Vector2 backgroundPadding;
    [Tooltip("Pixels per Unit multiplier for the background image")]
    [SerializeField] private float backgroundPPUMultiplier = 1;
    [Tooltip("Prefab of the UI line between the nodes (uses scripts from Unity UI Extensions)")]
    [SerializeField] private UILineRenderer uiLinePrefab;

    protected GameObject firstParent;
    protected GameObject mapParent;
    // ALL nodes:
    public readonly List<MapNode> MapNodes = new List<MapNode>();
    protected List<LineConnection> lineConnections = new List<LineConnection>();

    public Map Map { get; protected set; }

    protected void ClearMap()
    {
        mapObject.SetActive(false);

        foreach (ScrollRect scrollRect in new[] { scrollRect })
            foreach (Transform t in scrollRect.content)
                Destroy(t.gameObject);

        MapNodes.Clear();
        lineConnections.Clear();
    }

    public void ShowMap(Map m)
    {
        if (m == null)
        {
            Debug.LogWarning("Map was null in MapView.ShowMap()");
            return;
        }

        Map = m;

        ClearMap();

        CreateMapParent();

        CreateNodes(m.nodes);

        DrawLines();

        ResetNodesRotation();

        SetAttainableNodes();

        SetLineColors();

        CreateMapBackground(m);
    }

    public void ActiveMap(bool isActiveScroll)
    {
        if (isActiveScroll)
        {
            GameManager.GetInstance.soundManager.PlayBGM(BGMState.Map);
            mapObject.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            mapObject.transform.localPosition = new Vector3(0, 10000, 0);
        }
    }

    protected void CreateMapBackground(Map m)
    {
        GameObject backgroundObject = new GameObject("Background");
        backgroundObject.transform.SetParent(mapParent.transform);
        backgroundObject.transform.localScale = Vector3.one;
        RectTransform rt = backgroundObject.AddComponent<RectTransform>();
        Stretch(rt);
        rt.SetAsFirstSibling();
        rt.sizeDelta = backgroundPadding;

        Image image = backgroundObject.AddComponent<Image>();
        image.type = Image.Type.Sliced;
        image.sprite = background;
        image.pixelsPerUnitMultiplier = backgroundPPUMultiplier;
    }

    protected void CreateMapParent()
    {
        mapObject.SetActive(true);

        firstParent = new GameObject("OuterMapParent");
        firstParent.transform.SetParent(scrollRect.content);
        firstParent.transform.localScale = Vector3.one;
        RectTransform fprt = firstParent.AddComponent<RectTransform>();
        Stretch(fprt);

        mapParent = new GameObject("MapParentWithAScroll");
        mapParent.transform.SetParent(firstParent.transform);
        mapParent.transform.localScale = Vector3.one;
        RectTransform mprt = mapParent.AddComponent<RectTransform>();
        Stretch(mprt);

        SetMapLength();
        ScrollToOrigin();
    }

    private void SetMapLength()
    {
        RectTransform rt = scrollRect.content;
        Vector2 sizeDelta = rt.sizeDelta;
        float length = padding + Map.DistanceBetweenFirstAndLastLayers() * unitsToPixelsMultiplier;
        sizeDelta.y = length;
        rt.sizeDelta = sizeDelta;
    }

    private void ScrollToOrigin() => scrollRect.normalizedPosition = Vector2.zero;

    private static void Stretch(RectTransform tr)
    {
        tr.localPosition = Vector3.zero;
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.sizeDelta = Vector2.zero;
        tr.anchoredPosition = Vector2.zero;
    }

    protected void CreateNodes(IEnumerable<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            MapNode mapNode = CreateMapNode(node);
            MapNodes.Add(mapNode);
        }
    }

    protected MapNode CreateMapNode(Node node)
    {
        GameObject mapNodeObject = Instantiate(nodePrefab, mapParent.transform);
        MapNode mapNode = mapNodeObject.GetComponent<MapNode>();
        NodeBlueprint blueprint = GetBlueprint(node.nodeType);
        mapNode.SetUp(node, blueprint);
        mapNode.transform.localPosition = GetNodePosition(node);
        if(node.nodeType == NodeType.Boss)
        {
            mapNode.transform.localScale = new Vector3(3f, 3f, 3f);
        }
        return mapNode;
    }

    private Vector2 GetNodePosition(Node node)
    {
        float length = padding + Map.DistanceBetweenFirstAndLastLayers() * unitsToPixelsMultiplier;

        return new Vector2(-backgroundPadding.x / 2f, (padding - length) / 2f) +
                       node.position * unitsToPixelsMultiplier + correction;
    }

    public void SetAttainableNodes()
    {
        // first set all the nodes as unattainable/locked:
        foreach (MapNode node in MapNodes)
            node.SetState(NodeState.Locked);

        if (MapManager.GetInstance.CurrentMap.path.Count == 0)
        {
            // we have not started traveling on this map yet, set entire first layer as attainable:
            foreach (MapNode node in MapNodes.Where(n => n.Node.point.y == 0))
                node.SetState(NodeState.Attainable);
        }
        else
        {
            // we have already started moving on this map, first highlight the path as visited:
            foreach (Vector2Int point in MapManager.GetInstance.CurrentMap.path)
            {
                MapNode mapNode = GetNode(point);
                if (mapNode != null)
                    mapNode.SetState(NodeState.Visited);
            }

            Vector2Int currentPoint = MapManager.GetInstance.CurrentMap.path[MapManager.GetInstance.CurrentMap.path.Count - 1];
            Node currentNode = MapManager.GetInstance.CurrentMap.GetNode(currentPoint);

            // set all the nodes that we can travel to as attainable:
            foreach (Vector2Int point in currentNode.outgoing)
            {
                MapNode mapNode = GetNode(point);
                if (mapNode != null)
                    mapNode.SetState(NodeState.Attainable);
            }
        }
    }

    public void SetLineColors()
    {
        // set all lines to grayed out first:
        foreach (LineConnection connection in lineConnections)
            connection.SetColor(lineLockedColor);

        // set all lines that are a part of the path to visited color:
        // if we have not started moving on the map yet, leave everything as is:
        if (MapManager.GetInstance.CurrentMap.path.Count == 0)
            return;

        // in any case, we mark outgoing connections from the final node with visible/attainable color:
        Vector2Int currentPoint = MapManager.GetInstance.CurrentMap.path[MapManager.GetInstance.CurrentMap.path.Count - 1];
        Node currentNode = MapManager.GetInstance.CurrentMap.GetNode(currentPoint);

        foreach (Vector2Int point in currentNode.outgoing)
        {
            LineConnection lineConnection = lineConnections.FirstOrDefault(conn => conn.from.Node == currentNode &&
                                                                        conn.to.Node.point.Equals(point));
            lineConnection?.SetColor(lineVisitedColor);
        }

        if (MapManager.GetInstance.CurrentMap.path.Count <= 1) return;

        for (int i = 0; i < MapManager.GetInstance.CurrentMap.path.Count - 1; i++)
        {
            Vector2Int current = MapManager.GetInstance.CurrentMap.path[i];
            Vector2Int next = MapManager.GetInstance.CurrentMap.path[i + 1];
            LineConnection lineConnection = lineConnections.FirstOrDefault(conn => conn.@from.Node.point.Equals(current) &&
                                                                        conn.to.Node.point.Equals(next));
            lineConnection?.SetColor(lineVisitedColor);
        }
    }

    private void DrawLines()
    {
        foreach (MapNode node in MapNodes)
        {
            foreach (Vector2Int connection in node.Node.outgoing)
                AddLineConnection(node, GetNode(connection));
        }
    }

    private void ResetNodesRotation()
    {
        foreach (MapNode node in MapNodes)
            node.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// 라인 연결 부분 개선
    /// Screen Space - Camera 환경에서도 정상 작동하도록 anchorePosition 기반으로 처리
    /// </summary>
    private void AddLineConnection(MapNode from, MapNode to)
    {
        if (uiLinePrefab == null) return;
        
        UILineRenderer lineRenderer = Instantiate(uiLinePrefab, mapParent.transform, false);

        RectTransform fromRT = from.transform as RectTransform;
        RectTransform toRT = to.transform as RectTransform;
        RectTransform lineRT = lineRenderer.transform as RectTransform;

        Vector2 fromPos = fromRT.anchoredPosition;
        Vector2 toPos = toRT.anchoredPosition;

        // fromPos에서 offset만큼 떨어진 시작점
        Vector2 direction = (toPos - fromPos).normalized;
        Vector2 start = fromPos + direction * offsetFromNodes;

        // toPos에서 offset만큼 떨어진 끝점
        Vector2 end = toPos - direction * offsetFromNodes;

        // LineRenderer의 위치를 start와 end의 중간지점으로 설정
        Vector2 midPoint = (start + end) / 2f;
        lineRT.anchoredPosition = midPoint; 
        
        // 라인 포인트를 lineRenderer의 local space에서 계산하기 위해 start와 end를 lineRT 기준 local 좌표로 변환
        // 이미 parent가 동일하므로 anchorePosition 기준으로 보면 lineRT의 anchorePosition가 midPoint 이므로
        // start, end 상대좌표 = start - midPoint, end - midPoint
        Vector2 relativeStart = start - midPoint;
        Vector2 relativeEnd = end - midPoint;

        // lineRenderer는 points를 로컬 좌표로 받으므로 relativeStart에서 relativeEnd까지 lineSpacing 간격으로 점 생성
        List<Vector2> points = new List<Vector2>();
        Vector2 currentPoint = relativeStart;
        Vector2 targetDirection = (relativeEnd - relativeStart).normalized;
        float totalDistance = Vector2.Distance(relativeStart, relativeEnd);

        while (Vector2.Distance(currentPoint, relativeEnd) > lineSpacing)
        {
            currentPoint = Vector2.MoveTowards(currentPoint, relativeEnd, lineSpacing);
            points.Add(currentPoint);
        }

        // 마지막 점이 엔드포인트와 너무 떨어져 있지 않도록 end도 추가 (필요시)
        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], relativeEnd) > 0.01f)
            points.Add(relativeEnd);

        lineRenderer.Points = points.ToArray();

        lineConnections.Add(new LineConnection(lineRenderer, from, to));
    }

    private MapNode GetNode(Vector2Int p)
    {
        return MapNodes.FirstOrDefault(n => n.Node.point.Equals(p));
    }

    private MapConfig GetConfig(string configName)
    {
        return allMapConfigs.FirstOrDefault(c => c.name == configName);
    }

    private NodeBlueprint GetBlueprint(NodeType type)
    {
        return nodeBlueprints.FirstOrDefault(n => n.nodeType == type);
    }
}
