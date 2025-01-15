using UnityEngine;

[CreateAssetMenu(fileName = "Scenario_", menuName = "Scriptable Objects/Scenario/Dead")]
public class DeadScenario : Scenario
{
    public override void Play(int index = 0)
    {
        base.Play(index);
        BattleManager.GetInstance.TurnManager.CanContinueTurn = false;
        GameManager.GetInstance.soundManager.PlayBGM(BGMState.Title);
    }

    protected override void OnScenarioComplete()
    {
        DataCloud.IsScenarioMode = false;
        GameManager.GetInstance.ResetGame();
        GameManager.GetInstance.soundManager.StopAllSound();
        HelperUtilities.MoveScene(SceneType.Title);
    }
}
