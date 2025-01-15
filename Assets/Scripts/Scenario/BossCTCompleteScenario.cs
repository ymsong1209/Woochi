using UnityEditor.Build;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenario_", menuName = "Scriptable Objects/Scenario/BossCTComplete")]
public class BossCTCompleteScenario : Scenario
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

