using System.Collections;
using System.Collections.Generic;
using OneLine.Examples;
using UnityEngine;
using TMPro;
public class FoxFire_BlueFire : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {   
        GameObject dotCurebyDamagePrefab = BuffPrefabList[0];
        GameObject dotCureGameObject = Instantiate(dotCurebyDamagePrefab, transform);
        DotCureByDamageBuff dotCureBuff = dotCureGameObject.GetComponent<DotCureByDamageBuff>();
        dotCureBuff.BuffName = "푸른 불꽃";
        dotCureBuff.BuffDurationTurns = 3;
        dotCureBuff.IsAlwaysApplyBuff = true;
        dotCureBuff.BaseDotCureAmount = 3;
        dotCureBuff.MinDamage = SkillOwner.FinalStat.GetValue(StatType.MinDamage);
        dotCureBuff.MaxDamage = SkillOwner.FinalStat.GetValue(StatType.MaxDamage);
        dotCureBuff.DotCureAmount = 50;
        
        instantiatedBuffList.Add(dotCureGameObject);
      
        base.ActivateSkill(_opponent);
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * 50f / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * 50f / 100f);
        text.text = "푸른 불꽃\n" + 
                    "3턴 동안 아군의 체력을 턴 시작당\n" +
                    (int)(3 + minStat) + " ~ " + (int)(3 + maxStat) + " 만큼 회복";
    }
}
