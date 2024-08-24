using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Fascinate_Row3_P : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "홀리기";
        statDebuff.BuffDurationTurns = 2;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.changeStat.accuracy = -2;
        statDebuff.changeStat.speed = -2;
        
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "홀리기(광역)+\n" + "대상 전체에게 " + SkillSO.BaseMultiplier +"%의 피해를 주고 \n 홀림 부여";
    }
}
