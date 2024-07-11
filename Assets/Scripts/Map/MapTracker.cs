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
            // ó�� Map�� ������ ���� ù ��° Layer�� �ִ� ��� ��� ���� ����
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
        MapManager.GetInstance.SaveMap();
        view.SetAttainableNodes();
        view.SetLineColors();
        mapNode.ShowVisitAnimation();

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
