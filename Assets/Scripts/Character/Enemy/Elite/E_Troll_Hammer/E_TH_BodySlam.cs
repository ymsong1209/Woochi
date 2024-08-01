using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_TH_BodySlam : BaseSkill
{
    [SerializeField] private GameObject statBuffGameObject;//예리한 감각 버프.
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        //80%의 확률로 적을 기절시키는 디버프 부여
        GameObject stunDebuffPrefab = BuffPrefabList[0];
        GameObject stunDebuffGameObject = Instantiate(stunDebuffPrefab, transform);
        StunDeBuff stunDebuff = stunDebuffGameObject.GetComponent<StunDeBuff>();
        stunDebuff.BuffDurationTurns = 1;
        stunDebuff.ChanceToApplyBuff = 80;
        instantiatedBuffList.Add(stunDebuffGameObject);
        
        base.ActivateSkill(_Opponent);

        //스킬 적중시 예리한 감각 버프 부여
        if (SkillResult.isHit)
        {
            //씨름꾼의 끈기 버프 중첩시 지속 시간과 수치 중첩
            GameObject instantiatedStatbuff = Instantiate(statBuffGameObject, transform);
            StatBuff buff = instantiatedStatbuff.GetComponent<StatBuff>();
            buff.BuffName = "예리한 감각";
            buff.BuffDurationTurns = 3; //2턴동안 지속
            buff.changeStat.accuracy = 1;
            SkillOwner.ApplyBuff(SkillOwner,SkillOwner,buff);
        }
    }
}
