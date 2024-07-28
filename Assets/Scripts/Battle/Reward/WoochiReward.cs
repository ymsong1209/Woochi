using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Woochi")]
public class WoochiReward : Reward
{
    [SerializeField] private bool sorceryHeal;      // 도력 회복 여부
    [SerializeField] private bool randomStatUp;     // 랜덤 스탯 상승 여부
    [SerializeField] private bool allStatUp;        // 모든 스탯 상승 여부
    [SerializeField] private StatType statType;     // 증가시킬 스탯 타입
    [SerializeField] private int statUpAmount;      // 증가시킬 스탯 양
    public override void ApplyReward()
    {
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();

        // 도력 회복
        if (sorceryHeal)
        {
            woochi.SorceryPoints = woochi.MaxSorceryPoints;
        }

        // 랜덤 스탯 상승이라면 랜덤 스탯을 정함
        if(randomStatUp)
        {
            statType = (StatType)(Random.Range(0, (int)StatType.END));
        }

    }
}
