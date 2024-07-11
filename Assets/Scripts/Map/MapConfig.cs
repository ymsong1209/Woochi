using OneLine;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig_", menuName = "Scriptable Objects/Map/Config")]
public class MapConfig : ScriptableObject
{
    public string stageName;
    // 이 Map에 사용할 노드들(지역마다 특수한 노드들이 있을 수 있음)
    public List<NodeBlueprint> nodeBlueprints;

    [Tooltip("나타날 적들의 ID로 템플릿을 입력해주세요(ex. 3 3 4 4)")]
    [OneLineWithHeader]
    [SerializeField] private List<Template> normalTemplates;             // 적들이 등장하는 템플릿
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
}

[System.Serializable]
public class Template
{
    public int[] id;
}