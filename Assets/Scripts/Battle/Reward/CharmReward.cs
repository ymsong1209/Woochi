using UnityEngine;
[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Charm")]
public class CharmReward : Reward
{
    public override void ApplyReward()
    {
        if(!HelperUtilities.CanGetCharm())
        {
            return;
        }
    }
}
