using UnityEngine;

[CreateAssetMenu(fileName = "Scenario_", menuName = "Scriptable Objects/Scenario")]
public class Scenario : ScriptableObject
{
    public int ID;
    public Plot[] plots;

    private Plot currentPlot;

    public void Play()
    {
        StartPlot(0);
    }

    private void StartPlot(int index)
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

    /// <summary>
    /// 시나리오 끝
    /// </summary>
    private void OnScenarioComplete()
    {

    }

    public Plot CurrentPlot { get { return currentPlot; } }
}
