using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiAction_Charm : WoochiActionButton
{
    [SerializeField] WoochiCharmSelectionUI charmList;

    public override void Initialize(bool isEnable)
    {
        if (DataCloud.isMaintenance) return;

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
        if (DataCloud.isMaintenance) return;

        base.Deactivate();
        charmList.Deactivate();
    }

    public override void Interactable(bool isEnable)
    {
        if (DataCloud.isMaintenance) return;
        base.Interactable(isEnable);
    }
}
