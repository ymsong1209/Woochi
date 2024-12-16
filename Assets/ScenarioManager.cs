using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private Scenario[] scenarios;
    private Scenario currentScenario;

    public void Play(int index)
    {
        if (index < 0 || index >= scenarios.Length) return;

        currentScenario = scenarios[index];
        currentScenario.Play();
    }

    public Plot CurrentPlot
    {
        get { return currentScenario.CurrentPlot; }
    }
}
