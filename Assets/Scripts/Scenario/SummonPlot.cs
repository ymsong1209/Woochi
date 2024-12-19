using UnityEngine;

[CreateAssetMenu(fileName = "Plot_", menuName = "Scriptable Objects/Plot/Summon")]
public class SummonPlot : Plot
{
    [SerializeField] private GameObject prefab;
    
    public override void Effect()
    {
        base.Effect();
        
        BattleManager.GetInstance.AddAlly(prefab);
    }
}
