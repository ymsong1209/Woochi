using System.Linq;

public class MapManager : SingletonMonobehaviour<MapManager>
{
    public MapConfig[] mapConfigs;
    public MapConfig config;
    public MapView view;

    public Map CurrentMap { get; private set; }
    
    private void Start()
    {
        if(DataCloud.playerData.currentMap != null)
        {
            CurrentMap = new Map(DataCloud.playerData.currentMap);
            config = mapConfigs.FirstOrDefault(c => c.name.Equals(CurrentMap.configName));
            view.ShowMap(CurrentMap);

            // 보스까지 깬 맵이라면
            if(CurrentMap.path.Any(p => p.Equals(CurrentMap.GetBossNode().point)))
            {
                // ToDo : 다음 스테이지 넘어갈 수 있게
            }
        }
        else
        {
            GenerateNewMap();
        }
    }
    
    private void GenerateNewMap()
    {
        config = mapConfigs[0];
        Map map = MapGenerator.GetMap(config);
        CurrentMap = map;
        view.ShowMap(map);
        
        
        string mapLog = GenerateNewMapLog(map);
        Logger.Log(mapLog, "MapGeneration", "GeneratedMap");
    }
    
    private string GenerateNewMapLog(Map map)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine("----Generated Map----");
        sb.AppendLine($"MapConfig: {config.name}");

        // 각 계층별로 역순으로 처리 (10층 -> 0층)
        var groupedNodes = map.nodes.GroupBy(n => n.point.y).OrderByDescending(g => g.Key).ToList();

        for (int i = 0; i < groupedNodes.Count; i++)
        {
            var currentLayer = groupedNodes[i]; // 현재 계층
            sb.Append($"{currentLayer.Key}\t");
            foreach (var node in currentLayer)
            {
                sb.Append($"{node.nodeType} ");
            }
            sb.AppendLine();

            // 다음 계층(아래 층)과 연결 정보 표시
            if (i < groupedNodes.Count - 1)
            {
                var lowerLayer = groupedNodes[i + 1];
                foreach (var node in lowerLayer)
                {
                    foreach (var target in node.outgoing)
                    {
                        Node targetNode = map.GetNode(target);
                        if (targetNode != null && targetNode.point.y == currentLayer.Key) // 현재 층과 연결된 경우만 출력
                        {
                            sb.AppendLine($"    ({node.point}) {node.nodeType} -> ({target}) {targetNode.nodeType}");
                        }
                    }
                }
            }
        }

        sb.AppendLine("-------------------------");
        return sb.ToString();
    }



    public void SelectNode(MapNode _mapNode)
    {
        AllyFormation allies = BattleManager.GetInstance.Allies;
        allies.MoveNode();
        
        if (_mapNode.Node.nodeType == NodeType.Strange)
        {
            StrangeManager.GetInstance.InitializeStrange(_mapNode.Node.strangeID);
        }
        else
        {
            BattleManager.GetInstance.InitializeBattle(_mapNode.Node.enemyIDs, _mapNode.Node.abnormalID, _mapNode.Node.nodeType == NodeType.Elite);
        }
        view.ActiveMap(false);
    }

    public void CompleteNode()
    {
        MapPlayerTracker.Instance.Locked = false;
        view.ActiveMap(true);
    }

    public void SaveMap()
    {
        if (CurrentMap == null) return;
        
        DataCloud.playerData.currentMap = new Map(CurrentMap);
        GameManager.GetInstance.SaveData();
    }
}