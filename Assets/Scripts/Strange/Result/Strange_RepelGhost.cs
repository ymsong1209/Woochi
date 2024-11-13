using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/Ghost/Repel")]
public class Strange_RepelGhost : StrangeResult
{
    [SerializeField] private float percent = 0.1f;
    
    public override void ApplyEffect()
    {
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();
        
        woochi.UpdateSorceryPoints(percent, false);
    }
}
