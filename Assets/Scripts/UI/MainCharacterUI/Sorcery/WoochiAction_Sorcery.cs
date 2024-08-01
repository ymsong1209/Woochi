using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiAction_Sorcery : WoochiActionButton
{
   [SerializeField] WoochiSkillSelectionUI skilllist;

   public override void Initialize(bool isEnable)
   {
      if (DataCloud.isMaintenance) return;

      base.Initialize(isEnable);
      skilllist.Initialize(isEnable);
   }
   public override void Activate()
   {
      base.Activate();
      skilllist.Activate();
   }

   public override void Deactivate()
   {
      if (DataCloud.isMaintenance) return;

      base.Deactivate();
      skilllist.Deactivate();
   }

    public override void Interactable(bool isEnable)
    {
        if (DataCloud.isMaintenance) return;
        base.Interactable(isEnable);
    }
}
