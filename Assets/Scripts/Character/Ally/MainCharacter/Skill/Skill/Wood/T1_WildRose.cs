using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_WildRose : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject PoisonPrefab = BuffPrefabList[0];
        GameObject poisonGameObject = Instantiate(PoisonPrefab, transform);
        PoisonBuff poisonDebuff = poisonGameObject.GetComponent<PoisonBuff>();
        poisonDebuff.ChanceToApplyBuff = 50;
        poisonDebuff.PoisonStack = 2;
        instantiatedBuffList.Add(poisonGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "찔레꽃\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "50%의 확률로 단일 대상에게 중독 디버프 2만큼 부여";
    }
}
