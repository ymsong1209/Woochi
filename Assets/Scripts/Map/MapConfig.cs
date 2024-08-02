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
    [SerializeField] private RandomList<StrangeType> randomStrangeProbabilities;
    [Tooltip("나타날 기연의 ID를 입력")]
    [OneLineWithHeader]
    [SerializeField] private List<int> strangeTemplates;     
    [Tooltip("나타날 적들의 ID로 템플릿을 입력해주세요(ex. 3 3 4 4)")]
    [OneLineWithHeader]
    [SerializeField] private List<Template> normalTemplates;             // 일반 적들이 등장하는 템플릿
    [OneLineWithHeader]
    [SerializeField] private List<Template> eliteTemplates;              // 정예 적들이 등장하는 템플릿

    // 맵의 폭
    public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

    [OneLineWithHeader]
    public IntMinMax numOfPreBossNodes;     // 보스 노드 이전 layer에 있는 노드의 개수(최소, 최대)
    [OneLineWithHeader]
    public IntMinMax numOfStartingNodes;    // 시작 노드 layer에 있는 노드의 개수(최소, 최대)   

    [Tooltip("경로를 더 추가하고 싶은 경우 기입")]
    public int extraPaths;
    public List<MapLayer> layers;           // 지역의 layer 정보(layer마다 무슨 노드가 있는지 등)

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
        return eliteTemplates.Random().id;
    }

    public StrangeType GetStrangeType()
    {
        return randomStrangeProbabilities.Get();
    }

    public List<int> StrangeTemplates => strangeTemplates;
}

[System.Serializable]
public class Template
{
    public int[] id;
}

