using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/Damaged")]
public class Strange_Damaged : StrangeResult
{
    [SerializeField] private bool onlyWoochi = false;
    [SerializeField] private float percent = 0.1f;
    
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        
        if (onlyWoochi)
        {
            MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();

            int damage = (int)(woochi.Health.MaxHealth * percent);
            woochi.Health.Heal(-damage, false);
            // woochi.Health.ApplyDamage(damage, false, false);
        }
        else
        {
            List<BaseCharacter> allies = BattleManager.GetInstance.Allies.GetBattleCharacter();
            for(int i = 0; i < allies.Count; i++)
            {
                int damage = (int)(allies[i].Health.MaxHealth * percent);
                allies[i].Health.Heal(-damage, false);
                // allies[i].Health.ApplyDamage(damage, false, false);
            }
        }
    }
}
