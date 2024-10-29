using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/Hasuo")]
public class Strange_Hasuo : StrangeResult
{
    [SerializeField] private int sorceryPoints;     // 증가 도력
    [SerializeField] private int stat;          // 증가 피해

    public override void ApplyEffect()
    {
        MainCharacter mainCharacter = BattleManager.GetInstance.Allies.GetWoochi();

        if(mainCharacter)
        {
            mainCharacter.MaxSorceryPoints += sorceryPoints;
            mainCharacter.SorceryPoints += sorceryPoints;
            mainCharacter.rewardStat.minStat += stat;
            mainCharacter.rewardStat.maxStat += stat;
        }
    }
}
