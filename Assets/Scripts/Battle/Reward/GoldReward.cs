using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Gold")]
public class GoldReward : Reward
{
    [SerializeField] private int rewardGold;  

    public override bool ApplyReward()
    {
        HelperUtilities.AddGold(rewardGold);
        return true;
    }

    public override string GetResult()
    {
        resultTxt = $"{rewardGold} �帧�� ȹ���߽��ϴ�";
        return base.GetResult();
    }
}
