using UnityEngine;

/// <summary>
/// 소환수 회복 관련
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

        resultTxt = $"모든 소환수의 체력을 {healAmount}만큼 회복했습니다";
        return true;
    }
}
