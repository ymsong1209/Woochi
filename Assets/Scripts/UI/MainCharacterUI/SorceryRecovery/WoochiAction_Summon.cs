using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiAction_Summon : WoochiActionButton
{
    [SerializeField] AllyCardList allyUI;

    public override void Initialize(bool isEnable)
    {
        base.Initialize(isEnable);
        allyUI.gameObject.SetActive(isEnable);
    }

    public override void Activate()
    {
        base.Activate();
        allyUI.gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        allyUI.gameObject.SetActive(false);
    }

}
