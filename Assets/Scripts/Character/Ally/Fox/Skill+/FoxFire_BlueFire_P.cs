using System.Collections;
using System.Collections.Generic;
using OneLine.Examples;
using UnityEngine;
using TMPro;
public class FoxFire_BlueFire_P : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {   
        GameObject dotCurebyDamagePrefab = BuffPrefabList[0];
        GameObject dotCureGameObject = Instantiate(dotCurebyDamagePrefab, transform);
        DotCureByDamageBuff dotCureBuff = dotCureGameObject.GetComponent<DotCureByDamageBuff>();
        dotCureBuff.BuffName = "푸른 불꽃+";
        dotCureBuff.BuffDurationTurns = 3;
        dotCureBuff.IsAlwaysApplyBuff = true;
        dotCureBuff.DotCureAmount = 70;
        dotCureBuff.BaseDotCureAmount = 6;
        
        instantiatedBuffList.Add(dotCureGameObject);
      
        base.ActivateSkill(_opponent);
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * 70f / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * 70f / 100f);
        text.text = "푸른 불꽃+\n" + 
                    "3턴 동안 아군의 체력을 턴 시작당\n" +
                    (int)((6 + minStat)*0.7) + " ~ " + (int)((6 +maxStat) * 0.7) +" 만큼 회복";
    }
}
