using DG.Tweening;
using System.Linq;
using UnityEngine;

public class MapPlayerTracker : MonoBehaviour
{
    public bool lockAfterSelecting = false;
    public float enterNodeDelay = 1f;

    public MapView view;

    public static MapPlayerTracker Instance;

    public bool Locked { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SelectNode(MapNode mapNode)
    {
        if (Locked) return;

        if (MapManager.GetInstance.CurrentMap.path.Count == 0)
        {
            // 처음 Map에 들어왔을 때는 첫 번째 Layer에 있는 모든 노드 선택 가능
            if (mapNode.Node.point.y == 0)
                SendPlayerToNode(mapNode);
            else
                PlayWarningThatNodeCannotBeAccessed();
        }
        else
        {
            var currentPoint = MapManager.GetInstance.CurrentMap.path[MapManager.GetInstance.CurrentMap.path.Count - 1];
            var currentNode = MapManager.GetInstance.CurrentMap.GetNode(currentPoint);

            if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                SendPlayerToNode(mapNode);
            else
                PlayWarningThatNodeCannotBeAccessed();
        }
    }

    private void SendPlayerToNode(MapNode mapNode)
    {
        Locked = lockAfterSelecting;
        MapManager.GetInstance.CurrentMap.path.Add(mapNode.Node.point);
        view.SetAttainableNodes();
        view.SetLineColors();
        mapNode.ShowVisitAnimation();
        
        GameManager.GetInstance.soundManager.PlaySFX("Map_Click");
        DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
    }

    private static void EnterNode(MapNode mapNode)
    {
        MapManager.GetInstance.SelectNode(mapNode);
    }

    private void PlayWarningThatNodeCannotBeAccessed()
    {
        Debug.Log("Selected node cannot be accessed");
    }
}
