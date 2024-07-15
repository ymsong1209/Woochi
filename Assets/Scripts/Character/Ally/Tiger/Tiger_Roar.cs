using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Roar : BaseSkill
{
    [SerializeField] private GameObject roarBuffGameObject;
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.StatBuffName = "위축";
        statDebuff.BuffDurationTurns = 2;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.ChangeStat.accuracy = -5;
        statDebuff.ChangeStat.minStat = -5;
        statDebuff.ChangeStat.maxStat = -5;
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
        roarBuff.ChangeStat.defense = 5;
        // roarBuff.ChangeDefense = 5;
        SkillOwner.ApplyBuff(SkillOwner,roarBuff);
        Destroy(instantiatedRoarbuff);
    }
    
}
