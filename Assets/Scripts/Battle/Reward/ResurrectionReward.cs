using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Resurrection")]
public class ResurrectionReward : Reward
{
    public override bool ApplyReward()
    {
        AllyFormation allies = BattleManager.GetInstance.Allies;

        var deadCharacters = allies.GetAllies().FindAll(x => x.IsDead);

        if(deadCharacters.Count == 0)
        {
            return false;
        }
        else
        {
            int randomIndex = Random.Range(0, deadCharacters.Count);
            deadCharacters[randomIndex].Resurrect(true);
            resultTxt = $"{deadCharacters[randomIndex].Name}이(가) 부활했습니다";

            return true;
        }
    }
}
