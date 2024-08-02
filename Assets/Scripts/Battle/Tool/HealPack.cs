using UnityEngine;

public class HealPack : Tool
{
    [SerializeField] private int healAmount;

    public override void Use()
    {
        base.Use();

        AllyFormation allyFormation = BattleManager.GetInstance.Allies;
        var allies = allyFormation.GetAllies();
        foreach(var ally in allies)
        {
            if(ally.IsDead) continue;
            ally.Health.Heal(healAmount, false);
        }

        healAmount += 10;
    }

    public override string GetDescription()
    {
        description = $"체력을 {healAmount}만큼 회복합니다";
        return base.GetDescription();   
    }
}
