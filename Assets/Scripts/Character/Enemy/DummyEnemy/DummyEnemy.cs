using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : BaseEnemy
{
    public override void TriggerAI()
    {
        BaseCharacter ally = null;
        ally = BattleUtils.FindAllyFromIndex(0);
        BattleManager.GetInstance.SkillSelected(activeSkills[0]);
        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }
}
