using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T3_FireButterfly : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "갑옷 융해";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 70;
        statDeBuff.changeStat.defense = -3;
        instantiatedBuffList.Add(statDebuffGameObject);

        BaseCharacter opponent = BattleUtils.FindRandomEnemy(this);
        base.ActivateSkill(opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "화접\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "랜덤한 대상 2명에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "70%의 확률로 2턴동안 방어 -3만큼 부여";
    }
}