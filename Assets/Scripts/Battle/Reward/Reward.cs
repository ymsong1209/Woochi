using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward")]
public class Reward : ScriptableObject
{
    public Sprite sprite;
    public RareType rarity;
    public string rewardName;
    [TextArea(3, 10)]
    public string description;
    
    [Header("Popup Text")]
    [SerializeField] protected string resultTxt;
    [SerializeField] protected string errorTxt;
    
    public virtual bool ApplyReward()
    {
        return true;
    }
    
    public virtual string GetResult()
    {
        return resultTxt;
    }

    public virtual string GetError()
    {
        return errorTxt;
    }
}
