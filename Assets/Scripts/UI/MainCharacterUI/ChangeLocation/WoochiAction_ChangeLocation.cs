using UnityEngine;

public class WoochiAction_ChangeLocation : WoochiActionButton
{
    public override void Activate()
    {
        base.Activate();

        SelectSkill();
    }

    public override void Deactivate() 
    { 
        base.Deactivate();

        BattleManager.GetInstance.InitSelect();
    }

    void SelectSkill()
    {
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        MC_ChangeLocation changeLocation = mainCharacter.ChangeLocation;
        BattleManager.GetInstance.SkillSelected(changeLocation);
    }
}
