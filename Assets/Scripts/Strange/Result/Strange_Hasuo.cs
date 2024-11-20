using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/Hasuo")]
public class Strange_Hasuo : StrangeResult
{
    [SerializeField] private int sorceryPoints;     // 증가 도력
    [SerializeField] private int stat;          // 증가 피해

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        
        MainCharacter mainCharacter = BattleManager.GetInstance.Allies.GetWoochi();

        if(mainCharacter)
        {
            mainCharacter.MaxSorceryPoints += sorceryPoints;
            mainCharacter.SorceryPoints += sorceryPoints;
            mainCharacter.rewardStat.AddValue(StatType.MinDamage, stat);
            mainCharacter.rewardStat.AddValue(StatType.MaxDamage, stat);
        }
    }
}
