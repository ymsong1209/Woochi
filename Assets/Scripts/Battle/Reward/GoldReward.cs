using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Gold")]
public class GoldReward : Reward
{
    [Space, SerializeField] private int rewardGold;  

    public override bool ApplyReward()
    {
        resultTxt = $"{rewardGold} 흐름을 획득했습니다";
        HelperUtilities.AddGold(rewardGold);
        return true;
    }
}
