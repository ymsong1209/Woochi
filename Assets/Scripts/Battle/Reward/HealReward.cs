using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Heal")]
public class HealReward : Reward
{
    [SerializeField] private int healAmount;
    [SerializeField] private bool forWoochi;
    [SerializeField] private bool forAllies;

    public override void ApplyReward()
    {
        AllyFormation formation = BattleManager.GetInstance.Allies;
        var list = formation.GetCharacters();

        foreach(var character in list)
        {
            if(character.IsMainCharacter)
            {
                if (!forWoochi) continue;
                character.Health.Heal(healAmount, false);
            }
            else
            {
                if (!forAllies) continue;
                character.Health.Heal(healAmount, false);
            }
        }
    }
}
