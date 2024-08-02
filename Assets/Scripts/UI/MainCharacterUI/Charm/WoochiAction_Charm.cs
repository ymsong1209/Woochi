using UnityEngine;

public class WoochiAction_Charm : WoochiActionButton
{
    [SerializeField] WoochiCharmSelectionUI charmList;

    public override void Initialize(bool isEnable)
    {
        base.Initialize(isEnable);
        charmList.Initialize(isEnable);
    }
    public override void Activate()
    {
        base.Activate();
        charmList.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        charmList.Deactivate();
    }
}
