using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Gold")]
public class GoldReward : Reward
{
    [SerializeField] private int rewardGold;  

    public override bool ApplyReward()
    {
        resultTxt = $"{rewardGold} �帧�� ȹ���߽��ϴ�";
        HelperUtilities.AddGold(rewardGold);
        return true;
    }
}
