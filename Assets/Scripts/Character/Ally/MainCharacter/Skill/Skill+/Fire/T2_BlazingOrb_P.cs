using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T2_BlazingOrb_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject BurnPrefab = BuffPrefabList[0];
        GameObject burnGameObject = Instantiate(BurnPrefab, transform);
        BurnDebuff burnDebuff = burnGameObject.GetComponent<BurnDebuff>();
        burnDebuff.BuffDurationTurns = 3;
        burnDebuff.ChanceToApplyBuff = 70;
        instantiatedBuffList.Add(burnGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "불망울\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "랜덤한 대상 2명에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "70%의 확률로 화상 디버프 부여";
    }
}