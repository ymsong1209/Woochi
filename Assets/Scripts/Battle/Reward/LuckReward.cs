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
        resultTxt = $"���� {luck.turn}���� �������� {luck.value}��ŭ ����� �����մϴ�";
        return true;
    }
}
