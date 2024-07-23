using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TF_BodySlam : BaseSkill
{
    [SerializeField] private GameObject gritBuffGameObject;//씨름꾼의 끈기 버프.
    
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

        //스킬 적중시 씨름꾼의 끈기 버프 부여
        if (SkillResult.isHit)
        {
            //씨름꾼의 끈기 버프 중첩시 지속 시간과 수치 중첩
            GameObject instantiatedGritbuff = Instantiate(gritBuffGameObject, transform);
            StatBuff gritbuff = instantiatedGritbuff.GetComponent<StatBuff>();
            gritbuff.BuffName = "씨름꾼의 끈기";
            gritbuff.BuffDurationTurns = 2;
            gritbuff.ChangeStat.defense = 2;
            SkillOwner.ApplyBuff(SkillOwner,SkillOwner,gritbuff);
        }
    }
}
