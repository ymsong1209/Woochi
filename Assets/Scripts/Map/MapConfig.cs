using OneLine;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig_", menuName = "Scriptable Objects/Map/Config")]
public class MapConfig : ScriptableObject
{
    public string stageName;
    // �� Map�� ����� ����(�������� Ư���� ������ ���� �� ����)
    public List<NodeBlueprint> nodeBlueprints;

    [Tooltip("��Ÿ�� ������ ID�� ���ø��� �Է����ּ���(ex. 3 3 4 4)")]
    [OneLineWithHeader]
    [SerializeField] private List<Template> normalTemplates;             // ������ �����ϴ� ���ø�
    [SerializeField] private List<int> abnormals;                        // �� ������������ ��Ÿ�� �̻��

    // ���� ��
    public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

    [OneLineWithHeader]
    public IntMinMax numOfPreBossNodes;     // ���� ��� ���� layer�� �ִ� ����� ����(�ּ�, �ִ�)
    [OneLineWithHeader]
    public IntMinMax numOfStartingNodes;    // ���� ��� layer�� �ִ� ����� ����(�ּ�, �ִ�)   

    [Tooltip("��θ� �� �߰��ϰ� ���� ��� ����")]
    public int extraPaths;
    public List<MapLayer> layers;           // ������ layer ����(layer���� ���� ��尡 �ִ��� ��)

    public int GetAbnormal()
    {
        return abnormals.Random();
    }

    public int[] GetNormalEnemy()
    {
        return normalTemplates.Random().id;
    }
}

[System.Serializable]
public class Template
{
    public int[] id;
}