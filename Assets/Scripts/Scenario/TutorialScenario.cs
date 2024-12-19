using UnityEngine;

[CreateAssetMenu(fileName = "Scenario_", menuName = "Scriptable Objects/Scenario/Tutorial")]
public class TutorialScenario : Scenario
{
    protected override void OnScenarioComplete()
    {
        base.OnScenarioComplete();
        DataCloud.playerData.isFirstPlay = false;

        var characters = BattleManager.GetInstance.Allies.AllCharacter;
        for (int i = 0; i < characters.Count; ++i)
        {
            characters[i].InitializeHealth();
        }
        
        MapManager.GetInstance.LoadMap();
        MapManager.GetInstance.SaveMap();
    }
}
