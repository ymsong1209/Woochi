public class PlayerData
{
    // bool
    public bool isFirstPlay;    // ������ ���� ó�� ���� -> Ʃ�丮��
    public bool hasSaveData;    // ����� �����Ͱ� �ִ���

    // Battle
    public BattleData battleData;

    // Map Data
    public Map currentMap;

    // Default
    public int gold = 0;

    public PlayerData()
    {
        isFirstPlay = true;
        ResetData();
    }

    public void ResetData()
    {
        hasSaveData = false;

        battleData = new BattleData();
        currentMap = null;

        gold = 0;
    }
}
