using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/Ghost/Repel")]
public class Strange_RepelGhost : StrangeResult
{
    [SerializeField] private float percent = 0.1f;
    [SerializeField] private int gold = 1000;
    
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();
        
        woochi.UpdateSorceryPoints(percent, false);
        DataCloud.playerData.gold += gold;
    }
}
