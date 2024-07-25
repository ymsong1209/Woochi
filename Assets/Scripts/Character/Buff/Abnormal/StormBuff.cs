using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBuff : AbnormalBuff
{
    public override int ApplyTurnStartBuff()
    {
        float burnDamage = 2f;
        buffOwner.Health.ApplyDamage((int)Mathf.Round(burnDamage));

        return base.ApplyTurnStartBuff();
    }
}
