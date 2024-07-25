using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_Bind : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "옭아매기";
        statDeBuff.BuffDurationTurns = 4;
        statDeBuff.ChanceToApplyBuff = 80;
        statDeBuff.ChangeStat.speed = -2;
        statDeBuff.ChangeStat.minStat = -2;
        statDeBuff.ChangeStat.maxStat = -2;
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public MC_Bind()
    {
        requiredSorceryPoints = 70;
    }
}
