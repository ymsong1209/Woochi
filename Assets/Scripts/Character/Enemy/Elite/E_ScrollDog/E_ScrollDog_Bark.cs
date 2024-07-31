using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ScrollDog_Bark : BaseSkill
{
    [SerializeField] private GameObject evasionBuffGameObject;

    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "오싹한 짖기";
        statDebuff.BuffDurationTurns = 1;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.changeStat.accuracy = -5;
        instantiatedBuffList.Add(statDebuffGameObject);

        base.ActivateSkill(_Opponent);

        GameObject instantiatedEvasionbuff = Instantiate(evasionBuffGameObject, transform);
        StatBuff evasionBuff = instantiatedEvasionbuff.GetComponent<StatBuff>();
        evasionBuff.BuffName = "오싹한 짖기";
        evasionBuff.BuffDurationTurns = 2; //버프를 자신에게 걸고 이후 1턴동안 지속
        evasionBuff.changeStat.evasion = 5;
        SkillOwner.ApplyBuff(SkillOwner, SkillOwner, evasionBuff);
    }
}
