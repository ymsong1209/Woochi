using OneLine;
using System;
using UnityEngine;

[Serializable]
public struct Luck
{
    public int turn;
    public int value;
}

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Luck")]
public class LuckReward : Reward
{
    [OneLineWithHeader, SerializeField] Luck luck;

    public override bool ApplyReward()
    {
        DataCloud.playerData.luckList.Add(luck);
        resultTxt = $"다음 {luck.turn}번의 전투동안 {luck.value}만큼 행운이 증가합니다";
        return true;
    }
}
