using UnityEngine;

public class WoochiAction_Sorcery : WoochiActionButton
{
   [SerializeField] WoochiSkillSelectionUI skilllist;

   public override void Initialize(bool isEnable)
   {
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
      base.Deactivate();
      skilllist.Deactivate();
   }
}
