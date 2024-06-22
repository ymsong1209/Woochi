using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fascinate_Row3 : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.StatBuffName = "홀리기";
        statDebuff.BuffDurationTurns = 2;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.ChangeAccuracy = -10;
        statDebuff.ChangeSpeed = -10;
        
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
        BattleManager.GetInstance.MoveCharacter(SkillOwner, -1);
    }
}
