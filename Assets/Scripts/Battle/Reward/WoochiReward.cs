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

    public override bool ApplyReward()
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

        return true;
    }

    private void StatUp(MainCharacter woochi)
    {
        string result = string.Empty;

        if(randomStatUp)
        {
            StatType type = (StatType)Random.Range(0, (int)StatType.END);

            switch(type)
            {
                case StatType.Health:
                    result += "ü��";
                    addStat.maxHealth = statUpAmount;
                break;
                case StatType.Speed:
                    result += "�ӵ�";
                    addStat.speed = statUpAmount;
                break;
                case StatType.Defense:
                    result += "���";
                    addStat.defense = statUpAmount;
                break;
                case StatType.Crit:
                    result += "ġ��";
                    addStat.crit = statUpAmount;
                break;
                case StatType.Accuracy: 
                    result += "����";
                    addStat.accuracy = statUpAmount;
                break;
                case StatType.Evasion:
                    result += "ȸ��";
                    addStat.evasion = statUpAmount;
                break;
                case StatType.Resist:
                    result += "����";
                    addStat.resist = statUpAmount;
                break;
                case StatType.Damage:
                    result += "����";
                    addStat.minStat = statUpAmount;
                    addStat.maxStat = statUpAmount;
                break;
            }

            result += $"��(��) {statUpAmount} �����߽��ϴ�";
            HelperUtilities.ShowRewardToolResult(result);
        }

        woochi.rewardStat += addStat;
    }
}
