using OneLine;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig_", menuName = "Scriptable Objects/Map/Config")]
public class MapConfig : ScriptableObject
{
    public string stageName;
    // 이 Map에 사용할 노드들(지역마다 특수한 노드들이 있을 수 있음)
    public List<NodeBlueprint> nodeBlueprints;

    [OneLineWithHeader]
    [SerializeField] private List<RandomNode> randomNodes;

    [Tooltip("나타날 적들의 ID로 템플릿을 입력해주세요(ex. 3 3 4 4)")]
    [OneLineWithHeader]
    [SerializeField] private List<Template> normalTemplates;             // 일반 적들이 등장하는 템플릿
    [SerializeField] private List<Template> eliteTemplates;              // 정예 적들이 등장하는 템플릿
    [SerializeField] private List<int> abnormals;                        // 이 스테이지에서 나타날 이상들

    // 맵의 폭
    public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

    [OneLineWithHeader]
    public IntMinMax numOfPreBossNodes;     // 보스 노드 이전 layer에 있는 노드의 개수(최소, 최대)
    [OneLineWithHeader]
    public IntMinMax numOfStartingNodes;    // 시작 노드 layer에 있는 노드의 개수(최소, 최대)   

    [Tooltip("경로를 더 추가하고 싶은 경우 기입")]
    public int extraPaths;
    public List<MapLayer> layers;           // 지역의 layer 정보(layer마다 무슨 노드가 있는지 등)

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
        // 정예 적 템플릿 없어서 임의 처리
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
            Debug.LogError("Random Node의 확률의 합이 1이 아닙니다.");
        }
    }
}

[System.Serializable]
public class Template
{
    public int[] id;
}

/// <summary>
/// 랜덤으로 노드를 생성할 때 이 노드 타입이 어떤 확률로 나올지
/// </summary>
[System.Serializable]
public class RandomNode
{
    public NodeType nodeType;   // 노드 타입
    public float probability;   // 확률
}