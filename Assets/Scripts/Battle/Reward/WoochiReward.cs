using UnityEngine;

/// <summary>
/// ��ġ ���� ����
/// </summary>
[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Woochi")]
public class WoochiReward : Reward
{
    [Header("Heal")]
    [SerializeField] private bool heal;             // ü�� ȸ�� ����
    [SerializeField] private int healAmount;      // ü�� ȸ�� ��ġ
    [SerializeField] private bool sorceryHeal;      // ���� ȸ�� ����

    [Header("Random Stat Up")]
    [SerializeField] private bool randomStatUp;     // ���� ���� ��� ����
    [SerializeField] private int statUpAmount;      // ������ų ���� ��

    [Header("Add Stat")]
    [SerializeField] private Stat addStat;          // �߰��� ����   

    public override void ApplyReward()
    {
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();

        if(heal)
        {
            woochi.Health.Heal(healAmount, false);
        }

        // ���� ȸ��
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
