using UnityEngine;

[CreateAssetMenu(fileName = "Scenario_", menuName = "Scriptable Objects/Scenario/Normal")]
public class Scenario : ScriptableObject
{
    public int ID;
    public Plot[] plots;

    protected Plot currentPlot;

    public virtual void Play(int index = 0)
    {
        DataCloud.playerData.scenarioID = ID;
        DataCloud.IsScenarioMode = true;
        StartPlot(index);
    }

    protected void StartPlot(int index)
    {
        if(index < 0 || index >= plots.Length)
        {
            OnScenarioComplete();
            return;
        }
        
        currentPlot = plots[index];
        
        currentPlot.Play((nextIndex) =>
        {
            if(nextIndex < 0)
            {
                StartPlot(index + 1);
            }
            else
            {
                StartPlot(nextIndex);
            }
        });
    }

    protected virtual void OnScenarioComplete()
    {
        ScenarioManager.GetInstance.ActivateUI(false);
        DataCloud.IsScenarioMode = false;
        DataCloud.playerData.scenarioID++;
    }
    
    public Plot CurrentPlot => currentPlot;
}
