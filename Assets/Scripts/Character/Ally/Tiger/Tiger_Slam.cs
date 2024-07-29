using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tiger_Slam : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject stunDebuffPrefab = BuffPrefabList[0];
        GameObject stunDebuffGameObject = Instantiate(stunDebuffPrefab, transform);
        StunDeBuff stunDebuff = stunDebuffGameObject.GetComponent<StunDeBuff>();
        stunDebuff.BuffDurationTurns = 1;
        stunDebuff.ChanceToApplyBuff = 30;
        instantiatedBuffList.Add(stunDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "내려찍기\n" + "대상에게 " + SkillSO.BaseMultiplier +"%의 피해를 주고 \n 기절 부여";
    }
}
