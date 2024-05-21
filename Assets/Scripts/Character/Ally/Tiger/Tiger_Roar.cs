using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Roar : BaseSkill
{
    [SerializeField] private GameObject roarBuffGameObject;
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = bufflist[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.StatBuffName = "위축";
        statDebuff.BuffDurationTurns = 1;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.ChangeAccuracy = -5;
        statDebuff.ChangeMinStat = -5;
        statDebuff.ChangeMaxStat = -5;
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    
    protected override void ApplyMultiple()
    {
        base.ApplyMultiple();
        GameObject instantiatedRoarbuff = Instantiate(roarBuffGameObject, transform);
        StatBuff roarBuff = instantiatedRoarbuff.GetComponent<StatBuff>();
        roarBuff.StatBuffName = "산군의포효";
        roarBuff.BuffDurationTurns = 3;
        roarBuff.ChanceToApplyBuff = 100;
        roarBuff.ChangeDefense = 5;
        ApplyBuff(SkillOwner,roarBuff);
    }
    
}
