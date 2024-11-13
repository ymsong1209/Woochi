using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사나운 족자구 자신이 어디에 위치하든 아군 1, 2, 3, 4열 전체 대상에게 오싹한 짖기를 시전할 수 있다.
/// 오싹한 짖기를 시전하면 사나운 족자구는 1턴 동안 회피가 5만큼 증가한다.
/// 오싹한 짖기에 피격 당한 적은 1턴 동안 명중이 5 만큼 감소한다.
/// </summary>
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
        statDebuff.changeStat.SetValue(StatType.Accuracy, -5);
        instantiatedBuffList.Add(statDebuffGameObject);

        base.ActivateSkill(_Opponent);

        GameObject instantiatedEvasionbuff = Instantiate(evasionBuffGameObject, transform);
        StatBuff evasionBuff = instantiatedEvasionbuff.GetComponent<StatBuff>();
        evasionBuff.BuffName = "오싹한 짖기";
        evasionBuff.BuffDurationTurns = 2; //버프를 자신에게 걸고 이후 1턴동안 지속
        evasionBuff.changeStat.SetValue(StatType.Evasion, 5);
        SkillOwner.ApplyBuff(SkillOwner, SkillOwner, evasionBuff);
    }
}
