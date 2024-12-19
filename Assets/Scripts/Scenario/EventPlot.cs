using System;
using OneLine;
using UnityEngine;

[CreateAssetMenu(fileName = "Plot_", menuName = "Scriptable Objects/Plot/Event")]
public class EventPlot : Plot
{
    [Header("Turn Event")]
    [SerializeField] private bool isSetCanTurn = false;
    [SerializeField, OneLineWithHeader] private SetCanTurn[] setCanTurns;
    
    [Header("Time Event")]
    [SerializeField] private bool stopTime = false;

    [Header("Woochi Event")] 
    [SerializeField] private bool isGuageEmpty = false;
    
    public override void Effect()
    {
        base.Effect();
        
        if (isSetCanTurn)
        {
            for (int i = 0; i < setCanTurns.Length; ++i)
            {
                EventManager.GetInstance.onCanUseTurn?.Invoke(setCanTurns[i].ID, setCanTurns[i].canTurn);
            }
        }

        Time.timeScale = stopTime ? 0 : 1;

        if (isGuageEmpty)
        {
            BattleManager.GetInstance.Allies.GetWoochi().SorceryPoints = 0;
        }
    }
}

[Serializable]
public struct SetCanTurn
{
    public int ID;
    public bool canTurn;
}
