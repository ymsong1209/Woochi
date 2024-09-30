using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Haetae_DisasterImage_P : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "죄악환영+";
        statDebuff.BuffDurationTurns = 3;
        statDebuff.IsAlwaysApplyBuff = true;
        statDebuff.changeStat.accuracy = -4;
        statDebuff.changeStat.evasion = -4;
        statDebuff.changeStat.minStat = -4;
        statDebuff.changeStat.maxStat = -4;
        statDebuff.changeStat.speed = -4;
        instantiatedBuffList.Add(statDebuffGameObject);
        base.ActivateSkill(_Opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "죄악의 형상+\n" + 
                    "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" + 
                    "3턴동안 속도, 회피, 최소, 최대스탯, 속도 -4만큼 부여";
    }
}
