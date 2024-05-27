using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoochiAction_Sorcery : WoochiActionButton
{
   [SerializeField] WoochiSkillList skilllist;
   public override void Activate()
   {
      base.Activate();
      skilllist.Activate();
   }
}
