using OneLine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig_", menuName = "Scriptable Objects/Map/Config")]
public class MapConfig : ScriptableObject
{
    public string stageName;

    [SerializeField] private RandomList<NodeType> randomNodeTypes;
    [SerializeField] private RandomList<int> randomAbnormals;
    [Tooltip("��Ÿ�� ������ ID�� ���ø��� �Է����ּ���(ex. 3 3 4 4)")]
    [OneLineWithHeader]
    [SerializeField] private List<Template> normalTemplates;             // �Ϲ� ������ �����ϴ� ���ø�
    [SerializeField] private List<Template> eliteTemplates;              // ���� ������ �����ϴ� ���ø�

    // ���� ��
    public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

    [OneLineWithHeader]
    public IntMinMax numOfPreBossNodes;     // ���� ��� ���� layer�� �ִ� ����� ����(�ּ�, �ִ�)
    [OneLineWithHeader]
    public IntMinMax numOfStartingNodes;    // ���� ��� layer�� �ִ� ����� ����(�ּ�, �ִ�)   

    [Tooltip("��θ� �� �߰��ϰ� ���� ��� ����")]
    public int extraPaths;
    public List<MapLayer> layers;           // ������ layer ����(layer���� ���� ��尡 �ִ��� ��)

    public NodeType GetNodeType()
    {
        return randomNodeTypes.Get();
    }

    public int GetAbnormal()
    {
        return randomAbnormals.Get();
    }

    public int[] GetNormalEnemy()
    {
        return normalTemplates.Random().id;
    }

    public int[] GetEliteEnemy()
    {
        // ���� �� ���ø� ��� ���� ó��
        return normalTemplates.Random().id;
    }
}

[System.Serializable]
public class Template
{
    public int[] id;
}