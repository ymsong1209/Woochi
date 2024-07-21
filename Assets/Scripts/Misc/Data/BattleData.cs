using System.Collections.Generic;

[System.Serializable]
public class BattleData
{
    public List<int> allies;    // 플레이어(우치)가 소유하고 있는 소환수(우치 자기자신도 포함)
    public int[] formation;     // 전투에 참여할 포메이션(-1은 빈자리)

    public BattleData()
    {
        allies = new List<int>() { 0, 1, 2 };
        formation = new int[] { 0, -1, -1, -1};
    }
}
