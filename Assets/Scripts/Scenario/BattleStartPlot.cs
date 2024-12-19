using UnityEngine;

[CreateAssetMenu(fileName = "Plot_", menuName = "Scriptable Objects/Plot/BattleStart")]
public class BattleStartPlot : Plot
{
    [SerializeField] private int[] enemyIDs;
    
    public override void Effect()
    {
        base.Effect();
        
        BattleManager.GetInstance.InitializeBattle(enemyIDs);
    }
}
