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
        dotCureBuff.ChanceToApplyBuff = 100;
        dotCureBuff.DotCureAmount = 30;
        
        instantiatedBuffList.Add(dotCureGameObject);
      
        base.ActivateSkill(_opponent);
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "푸른 불꽃\n" + "아군의 체력을 턴 당 피해의 30%만큼 회복";
    }
}
