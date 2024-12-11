using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Heal")]
public class HealReward : Reward
{
    [Space, SerializeField] private int healAmount;

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
        GameManager.GetInstance.soundManager.PlaySFX("Heal_Use");
        return true;
    }
}
