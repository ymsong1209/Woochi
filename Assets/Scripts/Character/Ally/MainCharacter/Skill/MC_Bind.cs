using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MC_Bind : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "옭아매기";
        statDeBuff.BuffDurationTurns = 4;
        statDeBuff.ChanceToApplyBuff = 100;
        statDeBuff.changeStat.SetValue(StatType.MinDamage, -2);
        statDeBuff.changeStat.SetValue(StatType.MaxDamage, -2);
        statDeBuff.changeStat.SetValue(StatType.Speed, -2);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public MC_Bind()
    {
        requiredSorceryPoints = 40;
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "옭아매기\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "대상에게 " + SkillSO.BaseMultiplier +"%의 피해를 주고 뿌리 속박을 부여";
    }
}
