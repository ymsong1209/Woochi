using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCharm : BaseCharm
{
   
    public override void Activate(BaseCharacter opponent)
    {
        opponent.Health.Heal(10);
    }
}