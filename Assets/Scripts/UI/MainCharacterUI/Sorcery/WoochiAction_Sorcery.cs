using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiAction_Sorcery : WoochiActionButton
{
   [SerializeField] WoochiSkillSelectionUI skilllist;
   public override void Activate()
   {
      base.Activate();
      skilllist.Activate();
   }
}
