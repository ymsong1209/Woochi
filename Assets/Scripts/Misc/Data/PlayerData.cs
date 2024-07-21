using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    // bool
    public bool isFirstPlay;    // 게임을 완전 처음 시작 -> 튜토리얼

    // Battle
    public List<int> allies;    // 플레이어(우치)가 소유하고 있는 소환수(우치 자기자신도 포함)
    public int[] formation;     // 전투에 참여할 포메이션(-1은 빈자리)

    // Map Data
    public Map currentMap;

    // Default
    public int gold = 0;

    public PlayerData()
    {
        isFirstPlay = true;

        allies = new List<int>();
        formation = new int[] { 0, -1, -1, -1};
        currentMap = null;

    }

    public void ResetData()
    {
        allies = new List<int>();
        currentMap = null;
    }
}
