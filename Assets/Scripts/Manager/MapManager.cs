using System.Linq;

public class MapManager : SingletonMonobehaviour<MapManager>
{
    public MapConfig config;
    public MapView view;

    public Map CurrentMap { get; private set; }

    private void Start()
    {
        if(DataCloud.playerData.currentMap != null)
        {
            CurrentMap = new Map(DataCloud.playerData.currentMap);
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
        Map map = MapGenerator.GetMap(config);
        CurrentMap = map;
        view.ShowMap(map);
    }

    public void SelectNode(MapNode _mapNode)
    {
        BattleManager.GetInstance.InitializeBattle(_mapNode.Node.enemyIDs);
        view.FadeInOut(false);
    }

    public void CompleteNode()
    {
        SaveMap();
        view.FadeInOut(true);
    }

    public void SaveMap()
    {
        if (CurrentMap == null) return;

        DataCloud.playerData.currentMap = new Map(CurrentMap);
        DataCloud.SavePlayerData();
    }
}