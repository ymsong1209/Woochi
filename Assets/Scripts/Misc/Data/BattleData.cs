using System.Collections.Generic;

[System.Serializable]
public class BattleData
{
    public List<int> allies;    // 플레이어(우치)가 소유하고 있는 소환수(우치 자기자신도 포함)
    public int[] formation;     // 전투에 참여할 포메이션(-1은 빈자리)

    public List<StatData> statData;

    public BattleData()
    {
        allies = new List<int>() { 0, 1, 2 };
        formation = new int[] { 2, 0, 1, -1};   // 호랑이-우치-삼미호 순(시연버전)

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
