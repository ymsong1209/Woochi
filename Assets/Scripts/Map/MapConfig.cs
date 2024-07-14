using OneLine;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig_", menuName = "Scriptable Objects/Map/Config")]
public class MapConfig : ScriptableObject
{
    public string stageName;
    // �� Map�� ����� ����(�������� Ư���� ������ ���� �� ����)
    public List<NodeBlueprint> nodeBlueprints;

    [OneLineWithHeader]
    [SerializeField] private List<RandomNode> randomNodes;

    [Tooltip("��Ÿ�� ������ ID�� ���ø��� �Է����ּ���(ex. 3 3 4 4)")]
    [OneLineWithHeader]
    [SerializeField] private List<Template> normalTemplates;             // �Ϲ� ������ �����ϴ� ���ø�
    [SerializeField] private List<Template> eliteTemplates;              // ���� ������ �����ϴ� ���ø�
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

    public int[] GetEliteEnemy()
    {
        // ���� �� ���ø� ��� ���� ó��
        return normalTemplates.Random().id;
    }

    public NodeType GetRandomType()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulative = 0f;

        foreach(var randomNode in randomNodes)
        {
            cumulative += randomNode.probability;

            if (randomValue <= cumulative)
            {
                return randomNode.nodeType;
            }
        }

        return NodeType.Normal;
    }

    private void OnValidate()
    {
        if (randomNodes.Count == 0) return;

        float sum = 0f;

        for(int i = 0; i < randomNodes.Count; i++)
        {
            sum += randomNodes[i].probability;
        }

        if(sum != 1f)
        {
            Debug.LogError("Random Node�� Ȯ���� ���� 1�� �ƴմϴ�.");
        }
    }
}

[System.Serializable]
public class Template
{
    public int[] id;
}

/// <summary>
/// �������� ��带 ������ �� �� ��� Ÿ���� � Ȯ���� ������
/// </summary>
[System.Serializable]
public class RandomNode
{
    public NodeType nodeType;   // ��� Ÿ��
    public float probability;   // Ȯ��
}