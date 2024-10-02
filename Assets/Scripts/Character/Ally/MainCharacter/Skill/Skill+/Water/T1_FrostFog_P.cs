using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_FrostFog_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "시야 차단+";
        statDeBuff.BuffDurationTurns = 3;
        statDeBuff.ChanceToApplyBuff = 80;
        statDeBuff.changeStat.accuracy = -2;
        instantiatedBuffList.Add(statDebuffGameObject);

        BaseCharacter opponent = BattleUtils.FindRandomEnemy(this);
        base.ActivateSkill(opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "서리 안개+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "랜덤한 단일 대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "80%의 확률로 3턴동안 명중 -2만큼 부여";
    }
}