using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_UnholyWater : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        //100%의 확률로 부정한 물 디버프 부여
        GameObject elementalDebuffPrefab = BuffPrefabList[0];
        GameObject elementalDebuffGameObject = Instantiate(elementalDebuffPrefab, transform);
        ElementalStatDeBuff elementalDebuff = elementalDebuffGameObject.GetComponent<ElementalStatDeBuff>();
        elementalDebuff.BuffName = "부정한 물";
        elementalDebuff.BuffDurationTurns = 5;
        elementalDebuff.IsAlwaysApplyBuff = true;
        elementalDebuff.Element = SkillElement.Fire;
        elementalDebuff.ChangeStat = -6;
        instantiatedBuffList.Add(elementalDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
}
