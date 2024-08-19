using UnityEngine;
[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Charm")]
public class CharmReward : Reward
{
    public override bool ApplyReward()
    {
        return HelperUtilities.CanGetCharm();
    }
}
