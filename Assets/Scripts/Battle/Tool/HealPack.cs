using UnityEngine;

public class HealPack : Tool
{
    [SerializeField] private int healAmount;

    public override void Use()
    {
        base.Use();

        AllyFormation allyFormation = BattleManager.GetInstance.Allies;
        var allies = allyFormation.GetCharacters();
        foreach(var ally in allies)
        {
            ally.Health.Heal(healAmount, false);
        }
    }

    protected override string GetDescription()
    {
        description = $"ü���� {healAmount}��ŭ ȸ���մϴ�";
        return base.GetDescription();   
    }
}
