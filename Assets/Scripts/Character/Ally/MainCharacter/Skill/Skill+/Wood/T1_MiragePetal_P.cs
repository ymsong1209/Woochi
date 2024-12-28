using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_MiragePetal_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "환영 꽃가루+";
        statDeBuff.BuffDurationTurns = 3;
        statDeBuff.ChanceToApplyBuff = 80;
        statDeBuff.changeStat.SetValue(StatType.Defense, -10);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "환영 꽃가루+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 80%의 확률로 3턴동안 방어 -10만큼 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "단일 대상에게 80%의 롹률로\n" +
                                "3턴동안 방어 -10 부여";
    }
    
    
}