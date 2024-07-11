using OneLine;
using UnityEngine;

[System.Serializable]
public class MapLayer
{
    [Tooltip("�� Layer�� �⺻������ ���� ��尡 ���� �ϴ���")]
    public NodeType nodeType;

    [OneLineWithHeader] public FloatMinMax distanceFromPreviousLayer;   // ���� Layer�� �󸶳� ������ �ִ���
    [Tooltip("Layer�� �ִ� ��� �� �⺻������ ������ �ִ� �Ÿ�")]
    public float nodesApartDistance;
    [Tooltip("�� ���� 0�� ������ ������ ��������� 1�� ������ Layer�� ������ ����� �ִ� ����")]
    [Range(0f, 1f)] public float randomizePosition;
    [Tooltip("�� ���� 0�� ������ Layer�� ������ �⺻�� ����� �����ǰ� 1�� �������� �ٸ� ������ ������ �� ����")]
    [Range(0f, 1f)] public float randomizeNodes;
}
