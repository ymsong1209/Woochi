using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_MudFloor : MainCharacterSkill
{
    GameObject statDebuffPrefab;
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        GameObject statDebuffPrefab = BuffPrefabList[0];
        GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
        StatDeBuff statDeBuff = statDebuffGameObject.GetComponent<StatDeBuff>();
        statDeBuff.BuffName = "진흙투성이";
        statDeBuff.BuffDurationTurns = 2;
        statDeBuff.ChanceToApplyBuff = 70;
        statDeBuff.changeStat.SetValue(StatType.Speed, -2);
        instantiatedBuffList.Add(statDebuffGameObject);
        
        BaseCharacter opponent = BattleUtils.FindRandomEnemy(this);
        
        base.ActivateSkill(opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "감탕밭\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "랜덤한 단일 대상에게 70%의 확률로 2턴동안 속도 -2 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "랜덤한 단일 대상에게\n"+
                                "70%의 확률로\n" +
                                "2턴동안 속도 -2 부여";
    }
    
    public override void SetEnhancedSkillScrollDescription(int curskillid, TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        int enhancedSkillID = GameManager.GetInstance.Library.GetEnhancedSkillID(curskillid);
        MainCharacterSkill enhancedSkill = GameManager.GetInstance.Library.GetSkill(enhancedSkillID) as MainCharacterSkill;
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        MainCharacterSkillSO enhancedMainCharacterSkillSo = enhancedSkill.SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "랜덤한 단일 대상에게\n" +
                                "70%의 확률로\n" +
                                "2턴동안 속도 -2 부여\n" +
                                "-\n" +
                                "도력 <color=#FFFF00>" + enhancedMainCharacterSkillSo.RequiredSorceryPoints + "</color>을 소모\n" +
                                "랜덤한 단일 대상에게\n" +
                                "<color=#FFFF00>90%</color>의 확률로\n" +
                                "2턴동안 속도 <color=#FFFF00>-4</color> 부여";
    }
}
