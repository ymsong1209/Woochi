using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Haetae_DisasterImage : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "죄악환영";
        statDebuff.BuffDurationTurns = 3;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.changeStat.accuracy = -2;
        statDebuff.changeStat.evasion = -2;
        statDebuff.changeStat.minStat = -2;
        statDebuff.changeStat.maxStat = -2;
        statDebuff.changeStat.speed = -2;
        instantiatedBuffList.Add(statDebuffGameObject);
        base.ActivateSkill(_Opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "죄악의 형상\n" + "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" + "죄악환영 부여";
    }
}