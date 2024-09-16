using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class T1_MudFloor_P : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "진흙투성이+";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 90;
        statDeBuff.changeStat.speed = -4;
        instantiatedBuffList.Add(statDebuffGameObject);
        
        BaseCharacter opponent = FindRandomEnemy();
        
        base.ActivateSkill(opponent);
    }

    BaseCharacter FindRandomEnemy()
    {
        List<int> checkedIndices = new List<int>();
        // skillRadius에서 체크된 인덱스만 리스트에 추가
        for (int i = 0; i < SkillRadius.Length; i++)
        {
            if (SkillRadius[i])
            {
                checkedIndices.Add(i);
            }
        }

        return BattleUtils.FindRandomEnemy(checkedIndices.ToArray());
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "감탕밭+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "랜덤한 단일 대상에게 진흙투성이+ 디버프 부여";
    }
}
