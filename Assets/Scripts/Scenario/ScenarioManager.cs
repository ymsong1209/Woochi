using UnityEngine;

public class ScenarioManager : SingletonMonobehaviour<ScenarioManager>
{
    [SerializeField] private Scenario[] scenarios;
    private Scenario currentScenario;
    
    [HeaderTooltip("UI", "시나리오 UI")]
    [SerializeField] private ScenarioUI scenarioUI;

    [Header("Default")] 
    [SerializeField] private int plotIndex;
    
    public void Play(int index)
    {
        if (index < 0 || index >= scenarios.Length) return;
        
        ActivateUI(true);
        currentScenario = scenarios[index];
        currentScenario.Play(plotIndex);
    }
    
    public void SetUI(Plot plot)
    {
        scenarioUI.SetUI(plot);
    }
    
    public void ActivateUI(bool isShow)
    {
        scenarioUI.ActivateUI(isShow);
    }

    public void NextPlot(PlotEvent plotEvent)
    {
        if(DataCloud.IsScenarioMode == false) return;
        
        CurrentPlot.Next(plotEvent);
    }
    
    public Plot CurrentPlot => currentScenario.CurrentPlot;
}
