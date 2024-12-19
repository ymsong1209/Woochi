
public class Troll_Hammer_Tutorial : BaseEnemy
{
    private void Start()
    {
        canUseTurn = false;
    }

    public override void TriggerAI()
    {
        BaseCharacter ally = BattleManager.GetInstance.Allies.GetWoochi();
        BattleManager.GetInstance.SkillSelected(activeSkills[0]);

        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }
}
