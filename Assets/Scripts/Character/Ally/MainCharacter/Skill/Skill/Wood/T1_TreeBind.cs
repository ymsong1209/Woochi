using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_TreeBind : MainCharacterSkill
{
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "뿌리 속박";
        statDeBuff.BuffDurationTurns = 3;
        statDeBuff.ChanceToApplyBuff = 80;
        statDeBuff.changeStat.speed = -2;
        statDeBuff.changeStat.minStat = -2;
        statDeBuff.changeStat.maxStat = -2;
        instantiatedBuffList.Add(statDebuffGameObject);
        
        base.ActivateSkill(_opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        int minStat = (int)Mathf.Round(SkillOwner.FinalStat.minStat * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(SkillOwner.FinalStat.maxStat * SkillSO.BaseMultiplier / 100f);
        text.text = "옭아매기\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "80%의 확률로 3턴동안 속도, 최소, 최대스탯 -2 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        skillDescription.text = "도력 " + requiredSorceryPoints + "을 소모하여\n" +
                                "단일 대상에게 " + SkillSO.BaseMultiplier + "%피해를 주고\n" +
                                "80%의 확률로 3턴동안\n" +
                                "속도, 최소, 최대스탯 -2 부여";
    }
    
    public override void SetEnhancedSkillScrollDescription(int curskillid, TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        int enhancedSkillID = GameManager.GetInstance.Library.GetEnhancedSkillID(curskillid);
        MainCharacterSkill enhancedSkill = GameManager.GetInstance.Library.GetSkill(enhancedSkillID) as MainCharacterSkill;

        skillDescription.text = "도력 " + enhancedSkill.RequiredSorceryPoints + "을 소모하여\n" +
                                "단일 대상에게 <color=#FFFF00>" + SkillSO.BaseMultiplier + "</color>%피해를 주고\n" +
                                "<color=#FFFF00>100%</color>의 확률로 <color=#FFFF00>4턴</color>동안\n" +
                                "속도, 최소, 최대스탯 -2 부여";
    }
}
