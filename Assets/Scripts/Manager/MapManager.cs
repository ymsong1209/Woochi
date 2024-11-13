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
            CurrentMap = DataCloud.playerData.currentMap;
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

        DataCloud.playerData.currentMap = CurrentMap;
        GameManager.GetInstance.SaveData();
    }
}