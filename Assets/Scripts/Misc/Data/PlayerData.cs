public class PlayerData
{
    // bool
    public bool isFirstPlay;    // 게임을 완전 처음 시작 -> 튜토리얼
    public bool hasSaveData;    // 저장된 데이터가 있는지

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
