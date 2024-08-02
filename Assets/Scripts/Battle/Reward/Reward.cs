using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward")]
public class Reward : ScriptableObject
{
    public Sprite sprite;
    public RareType rarity;
    public string rewardName;
    [TextArea(3, 10)]
    public string description;

    public virtual void ApplyReward()
    {

    }
    
}
