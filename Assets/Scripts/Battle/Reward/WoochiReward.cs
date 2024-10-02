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
        if(randomStatUp)
        {
            StatType type = (StatType)Random.Range(0, (int)StatType.END);

            resultTxt += "��ġ�� ";
            switch(type)
            {
                case StatType.Health:
                    resultTxt += "ü��";
                    addStat.maxHealth = statUpAmount;
                break;
                case StatType.Speed:
                    resultTxt += "�ӵ�";
                    addStat.speed = statUpAmount;
                break;
                case StatType.Defense:
                    resultTxt += "���";
                    addStat.defense = statUpAmount;
                break;
                case StatType.Crit:
                    resultTxt += "ġ��";
                    addStat.crit = statUpAmount;
                break;
                case StatType.Accuracy: 
                    resultTxt += "����";
                    addStat.accuracy = statUpAmount;
                break;
                case StatType.Evasion:
                    resultTxt += "ȸ��";
                    addStat.evasion = statUpAmount;
                break;
                case StatType.Resist:
                    resultTxt += "����";
                    addStat.resist = statUpAmount;
                break;
                case StatType.Damage:
                    resultTxt += "����";
                    addStat.minStat = statUpAmount;
                    addStat.maxStat = statUpAmount;
                break;
            }

            resultTxt += $"��(��) {statUpAmount} �����߽��ϴ�";
        }

        woochi.rewardStat += addStat;
    }
}
