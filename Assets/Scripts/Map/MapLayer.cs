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
    [Tooltip("이 값이 0에 가까우면 직선에 가까워지고 1에 가까우면 Layer의 노드들이 흩어져 있는 느낌")]
    [Range(0f, 1f)] public float randomizePosition;
    [Tooltip("이 값이 0에 가까우면 Layer의 노드들이 기본값 노드들로 구성되고 1에 가까울수록 다른 노드들이 등장할 수 있음")]
    [Range(0f, 1f)] public float randomizeNodes;
}
