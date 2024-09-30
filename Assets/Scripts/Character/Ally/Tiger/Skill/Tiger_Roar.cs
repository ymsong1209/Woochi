using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tiger_Roar : BaseSkill
{
    [SerializeField] private GameObject roarBuffGameObject;
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "위축";
        statDebuff.BuffDurationTurns = 2;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.changeStat.accuracy = -2;
        statDebuff.changeStat.speed = -2;
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_Opponent);

        if (SkillResult.IsAnyHit())
        {
            GameObject instantiatedRoarbuff = Instantiate(roarBuffGameObject, transform);
            StatBuff roarBuff = instantiatedRoarbuff.GetComponent<StatBuff>();
            roarBuff.BuffName = "산군의 위엄";
            roarBuff.BuffDurationTurns = 3;
            roarBuff.IsAlwaysApplyBuff = true;
            roarBuff.changeStat.defense = 2;
            SkillOwner.ApplyBuff(SkillOwner,SkillOwner,roarBuff);
        }
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "산군의 포효\n" 
                    + "대상 전체에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n"
                    + "100% 확률로 명중, 속도 -2만큼 부여\n" 
                    + "자신에게 3턴동안 방어 +2만큼 부여";
    }
}
