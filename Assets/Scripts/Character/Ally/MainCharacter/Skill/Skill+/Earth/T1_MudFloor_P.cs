using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class T1_MudFloor_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "진흙투성이+";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 90;
        statDeBuff.BuffStackType = BuffStackType.ResetDuration;
        statDeBuff.changeStat.SetValue(StatType.Speed, -5);
        statDeBuff.changeStat.SetValue(StatType.Evasion, -5);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        BaseCharacter opponent = BattleUtils.FindRandomEnemy(this);
        
        base.ActivateSkill(opponent);
    }
    
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "감탕밭+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "랜덤한 단일 대상에게 90%의 확률로\n" +
                    "2턴동안 속도 -5, 회피 -5 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "랜덤한 단일 대상에게\n"+
                                "90%의 확률로 2턴동안\n" +
                                "속도 -5, 회피 -5 부여";
    }
}
