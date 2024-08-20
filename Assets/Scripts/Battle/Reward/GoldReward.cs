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
}
