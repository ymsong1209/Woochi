using UnityEngine;

public class WoochAction_SorceryRecovery : WoochiActionButton
{
    [SerializeField] WoochiRecoveryUI recoveryUI;
    
    public override void Activate()
    {
        base.Activate();
        recoveryUI.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        recoveryUI.Deactivate();
    }
}
