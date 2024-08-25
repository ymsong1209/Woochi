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
        dotCureBuff.BuffName = "푸른 불꽃";
        dotCureBuff.BuffDurationTurns = 3;
        dotCureBuff.ChanceToApplyBuff = 100;
        dotCureBuff.DotCureAmount = 40;
        
        instantiatedBuffList.Add(dotCureGameObject);
      
        base.ActivateSkill(_opponent);
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * 40f / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * 40f / 100f);
        text.text = "푸른 불꽃+\n" + "아군의 체력을 턴 당 " + minStat + " ~ " + maxStat + "만큼 회복";
    }
}
