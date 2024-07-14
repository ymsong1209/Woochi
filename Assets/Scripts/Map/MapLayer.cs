using OneLine;
using UnityEngine;

[System.Serializable]
public class MapLayer
{
    [Tooltip("이 Layer에 기본적으로 무슨 노드가 들어가야 하는지")]
    public NodeType nodeType;

    [OneLineWithHeader] public FloatMinMax distanceFromPreviousLayer;   // 이전 Layer와 얼마나 떨어져 있는지

    [Tooltip("Layer에 있는 노드 간 기본적으로 떨어져 있는 거리")]
    public float nodesApartDistance;

    [Tooltip("이 Layer에 랜덤 노드 생성을 허용할 지 말지")]
    public bool isRandomNode = true;

    [Tooltip("이 Layer에 나오는 적들은 고정인지")]
    public bool isRandomEnemy = true;

    [Tooltip("고정이라면 어떤 적이 나와야 하는지")]
    [OneLineWithHeader] public int[] enemyIDs;

    [Tooltip("이 값이 0에 가까우면 직선에 가까워지고 1에 가까우면 Layer의 노드들이 흩어져 있는 느낌")]
    [Range(0f, 1f)] public float randomizePosition;
}
