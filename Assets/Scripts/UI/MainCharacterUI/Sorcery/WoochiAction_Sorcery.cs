using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiAction_Sorcery : WoochiActionButton
{
   [SerializeField] WoochiSkillSelectionUI skilllist;

   public override void Initialize()
   {
      base.Initialize();
      skilllist.Initialize();
   }
   public override void Activate()
   {
      base.Activate();
      skilllist.Activate();
   }

   public override void Deactivate()
   {
      base.Deactivate();
      skilllist.Deactivate();
   }
}
