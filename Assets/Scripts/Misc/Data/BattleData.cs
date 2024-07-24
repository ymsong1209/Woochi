using System.Collections.Generic;

[System.Serializable]
public class BattleData
{
    public List<int> allies;    // �÷��̾�(��ġ)�� �����ϰ� �ִ� ��ȯ��(��ġ �ڱ��ڽŵ� ����)
    public int[] formation;     // ������ ������ �����̼�(-1�� ���ڸ�)

    public List<CharacterInfoData> characterInfoList;

    public BattleData()
    {
        allies = new List<int>() { 0, 1, 2 };
        formation = new int[] { 2, 0, 1, -1};   // ȣ����-��ġ-���ȣ ��(�ÿ�����)

        characterInfoList = new List<CharacterInfoData>();
    }
}

[System.Serializable]
public class CharacterInfoData
{
    public int ID;
    public Stat baseStat;
    public Stat rewardStat;
    public Health health;

    public CharacterInfoData()
    {
        ID = -1;
        baseStat = null;
        rewardStat = null;
        health = null;
    }

    public CharacterInfoData(int _ID, Stat _baseStat, Stat _rewardStat, Health _health)
    {
        ID = _ID;
        baseStat = _baseStat;
        rewardStat = _rewardStat;
        health = _health;
    }
}
