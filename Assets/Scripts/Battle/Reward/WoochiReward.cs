using UnityEngine;

/// <summary>
/// 우치 전용 보상
/// </summary>
[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Woochi")]
public class WoochiReward : Reward
{
    [Header("Heal")]
    [SerializeField] private bool heal;             // 체력 회복 여부
    [SerializeField] private int healAmount;      // 체력 회복 수치
    [SerializeField] private bool sorceryHeal;      // 도력 회복 여부

    [Header("Random Stat Up")]
    [SerializeField] private bool randomStatUp;     // 랜덤 스탯 상승 여부
    [SerializeField] private int statUpAmount;      // 증가시킬 스탯 양

    [Header("Add Stat")]
    [SerializeField] private Stat addStat;          // 추가할 스탯   

    public override void ApplyReward()
    {
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();

        if(heal)
        {
            woochi.Health.Heal(healAmount, false);
        }

        // 도력 회복
        if (sorceryHeal)
        {
            woochi.SorceryPoints = woochi.MaxSorceryPoints;
        }

        StatUp(woochi);
    }

    private void StatUp(MainCharacter woochi)
    {
        if(randomStatUp)
        {
            StatType type = (StatType)Random.Range(0, (int)StatType.END);

            switch(type)
            {
                case StatType.Health:
                    addStat.maxHealth = statUpAmount;
                break;
                case StatType.Speed:
                    addStat.speed = statUpAmount;
                break;
                case StatType.Defense:
                    addStat.defense = statUpAmount;
                break;
                case StatType.Crit:
                    addStat.crit = statUpAmount;
                break;
                case StatType.Accuracy: 
                    addStat.accuracy = statUpAmount;
                break;
                case StatType.Evasion:
                    addStat.evasion = statUpAmount;
                break;
                case StatType.Resist:
                    addStat.resist = statUpAmount;
                break;
                case StatType.Damage:
                    addStat.minStat = statUpAmount;
                    addStat.maxStat = statUpAmount;
                break;
            }
        }

        woochi.rewardStat += addStat;
    }
}
