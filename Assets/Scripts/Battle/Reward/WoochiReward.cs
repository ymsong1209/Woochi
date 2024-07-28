using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Woochi")]
public class WoochiReward : Reward
{
    [SerializeField] private bool sorceryHeal;      // ���� ȸ�� ����
    [SerializeField] private bool randomStatUp;     // ���� ���� ��� ����
    [SerializeField] private bool allStatUp;        // ��� ���� ��� ����
    [SerializeField] private StatType statType;     // ������ų ���� Ÿ��
    [SerializeField] private int statUpAmount;      // ������ų ���� ��
    public override void ApplyReward()
    {
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();

        // ���� ȸ��
        if (sorceryHeal)
        {
            woochi.SorceryPoints = woochi.MaxSorceryPoints;
        }

        // ���� ���� ����̶�� ���� ������ ����
        if(randomStatUp)
        {
            statType = (StatType)(Random.Range(0, (int)StatType.END));
        }

    }
}
