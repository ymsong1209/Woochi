using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tiger_Roar_P : BaseSkill
{
    [SerializeField] private GameObject roarBuffGameObject;
    
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDebuff.BuffName = "위축+";
        statDebuff.BuffDurationTurns = 2;
        statDebuff.ChanceToApplyBuff = 100;
        statDebuff.BuffStackType = BuffStackType.ResetDuration;
        statDebuff.changeStat.SetValue(StatType.Accuracy, -4);
        statDebuff.changeStat.SetValue(StatType.Speed, -4);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_Opponent);

        if (SkillResult.IsAnyHit())
        {
            GameObject instantiatedRoarbuff = Instantiate(roarBuffGameObject, transform);
            StatBuff roarBuff = instantiatedRoarbuff.GetComponent<StatBuff>();
            roarBuff.BuffName = "산군의 위엄+";
            roarBuff.BuffDurationTurns = 3;
            roarBuff.ChanceToApplyBuff = 100;
            roarBuff.BuffStackType = BuffStackType.ExtendDuration;
            roarBuff.changeStat.SetValue(StatType.Defense, 20);
            SkillOwner.ApplyBuff(SkillOwner,SkillOwner,roarBuff);
        }
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "산군의 포효+\n" + 
                    "대상 전체에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" + 
                    "100%의 확률로 명중, 속도 -4만큼 부여\n" + 
                    "자신에게 3턴동안 방어 +20 부여";
    }
}
