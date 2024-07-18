using DataTable;
using UnityEngine;

[CreateAssetMenu(fileName = "Abnormal", menuName = "Scriptable Objects/Map/Abnormal")]
public class Abnormal : ScriptableObject
{
    [Tooltip("ID만 채워주면 자동으로 데이터 초기화")]
    public int ID;
    [ReadOnly(true)] public string Name;
    [ReadOnly(true)] public int cost;

    public void Initialize()
    {
        AbnormalData data = AbnormalData.GetDictionary()[ID];

        Name = data.abnormalName;
        cost = data.cost;
    }
}
