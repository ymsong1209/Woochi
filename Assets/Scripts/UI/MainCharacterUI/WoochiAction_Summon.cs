using UnityEngine;

public class WoochiAction_Summon : WoochiActionButton
{
    [SerializeField] AllyCardList allyUI;

    public override void Initialize(bool isEnable)
    {
        base.Initialize(isEnable);
    }

    public override void Activate()
    {
        base.Activate();

        allyUI.gameObject.SetActive(true);
        allyUI.canSummon = true;
        BattleManager.GetInstance.EnableDummy();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        allyUI.gameObject.SetActive(false);
        allyUI.canSummon = false;
        BattleManager.GetInstance.DisableDummy();
    }

}
