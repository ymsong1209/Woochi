using UnityEngine;

/// <summary>
/// ��ȯ�� ȸ�� ����
/// </summary>
[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Heal")]
public class HealReward : Reward
{
    [SerializeField] private int healAmount;

    public override bool ApplyReward()
    {
        AllyFormation formation = BattleManager.GetInstance.Allies;
        var list = formation.GetAllies();

        foreach(var character in list)
        {
            if (character.IsDead) continue;
            character.Health.Heal(healAmount, false);
        }

        resultTxt = $"��� ��ȯ���� ü���� {healAmount}��ŭ ȸ���߽��ϴ�";
        return true;
    }
}
