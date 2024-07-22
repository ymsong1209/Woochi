using System.Collections.Generic;

[System.Serializable]
public class BattleData
{
    public List<int> allies;    // �÷��̾�(��ġ)�� �����ϰ� �ִ� ��ȯ��(��ġ �ڱ��ڽŵ� ����)
    public int[] formation;     // ������ ������ �����̼�(-1�� ���ڸ�)

    public List<StatData> statData;

    public BattleData()
    {
        allies = new List<int>() { 0, 1, 2 };
        formation = new int[] { 2, 0, 1, -1};   // ȣ����-��ġ-���ȣ ��(�ÿ�����)

        statData = new List<StatData>();
    }

    public void AddStatData(StatData newStatData)
    {
        foreach(var stat in statData)
        {
            if (stat.ID == newStatData.ID)
            {
                stat.stat = new Stat(newStatData.stat);
                return;
            }
        }

        statData.Add(newStatData);
    }
}

[System.Serializable]
public class StatData
{
    public int ID;
    public Stat stat;

    public StatData()
    {
        ID = -1;
        stat = new Stat();
    }

    public StatData(int _ID, Stat _stat)
    {
        ID = _ID;
        stat = new Stat(_stat);
    }
}
