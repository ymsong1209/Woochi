using OneLine;
using UnityEngine;

[System.Serializable]
public class MapLayer
{
    [Tooltip("�� Layer�� �⺻������ ���� ��尡 ���� �ϴ���")]
    public NodeType nodeType;

    [OneLineWithHeader] public FloatMinMax distanceFromPreviousLayer;   // ���� Layer�� �󸶳� ������ �ִ���

    [Tooltip("Layer�� �ִ� ��� �� �⺻������ ������ �ִ� �Ÿ�")]
    public float nodesApartDistance = 2.5f;

    [Tooltip("�� Layer�� ���� ��� ������ ����� �� ����")]
    public bool isRandomNode = true;

    [Tooltip("�� Layer�� ������ ������ ��������")]
    public bool isRandomEnemy = true;

    [Tooltip("�����̶�� � ���� ���;� �ϴ���")]
    public int[] enemyIDs;

    [Tooltip("�� ���� 0�� ������ ������ ��������� 1�� ������ Layer�� ������ ����� �ִ� ����")]
    [Range(0f, 1f)] public float randomizePosition;
}
