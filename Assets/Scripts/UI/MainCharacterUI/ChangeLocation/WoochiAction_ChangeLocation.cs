using UnityEngine;

public class WoochiAction_ChangeLocation : WoochiActionButton
{
    public override void Activate()
    {
        base.Activate();
        icon.color = Color.white;

        SelectSkill();
    }

    public override void Deactivate() 
    { 
        base.Deactivate();
        icon.color = Color.grey;

        BattleManager.GetInstance.InitSelect();
    }

    void SelectSkill()
    {
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        MC_ChangeLocation changeLocation = mainCharacter.ChangeLocation;
        BattleManager.GetInstance.SkillSelected(changeLocation);
    }
}
