using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDog_Bite : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
       
        GameObject bleedDebuffPrefab = BuffPrefabList[0];
        GameObject bleedDebuffGameObject = Instantiate(bleedDebuffPrefab, transform);
        BleedDeBuff bleedDebuff = bleedDebuffGameObject.GetComponent<BleedDeBuff>();
        bleedDebuff.BuffDurationTurns = 3;
        bleedDebuff.ChanceToApplyBuff = 65;
        bleedDebuff.BleedPercent = 3;
        instantiatedBuffList.Add(bleedDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
}
