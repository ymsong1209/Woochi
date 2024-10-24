using UnityEngine;

/// <summary>
/// 소환수 회복 관련
/// </summary>
[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Heal")]
public class HealReward : Reward
{
    [SerializeField] private int healAmount;

    public override void ApplyReward()
    {
        AllyFormation formation = BattleManager.GetInstance.Allies;
        var list = formation.GetAllies();

        foreach(var character in list)
        {
            if (character.IsDead) continue;
            character.Health.Heal(healAmount, false);
        }
    }
}
