using UnityEngine;

public class WoochAction_SorceryRecovery : WoochiActionButton
{
    [SerializeField] WoochiRecoveryUI recoveryUI;
    
    public override void Initialize(bool isEnable)
    {
        if (DataCloud.isMaintenance) return;

        base.Initialize(isEnable);
        recoveryUI.Initialize();
    }

    public override void Activate()
    {
        base.Activate();
        recoveryUI.Activate();
    }

    public override void Deactivate()
    {
        if (DataCloud.isMaintenance) return;

        base.Deactivate();
        recoveryUI.Deactivate();
    }

    public override void Interactable(bool isEnable)
    {
        if(DataCloud.isMaintenance) return;
        base.Interactable(isEnable);
    }
}
