using UnityEngine;

[CreateAssetMenu(fileName = "Scenario_", menuName = "Scriptable Objects/Scenario/Tutorial")]
public class TutorialScenario : Scenario
{
    protected override void OnScenarioComplete()
    {
        base.OnScenarioComplete();
        DataCloud.playerData.isFirstPlay = false;
        
        MapManager.GetInstance.LoadMap();
        MapManager.GetInstance.SaveMap();
    }
}
