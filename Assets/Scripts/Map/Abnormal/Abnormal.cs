using DataTable;
using UnityEngine;

[CreateAssetMenu(fileName = "Abnormal", menuName = "Scriptable Objects/Map/Abnormal")]
public class Abnormal : ScriptableObject
{
    [Tooltip("ID�� ä���ָ� �ڵ����� ������ �ʱ�ȭ")]
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
