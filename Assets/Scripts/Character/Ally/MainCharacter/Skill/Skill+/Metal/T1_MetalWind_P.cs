using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T1_MetalWind_P : MainCharacterSkill
{
    [SerializeField] private GameObject DefenseBuffGameObject;
    public override void ActivateSkill(BaseCharacter _opponent)
    {

        base.ActivateSkill(_opponent);
        int random = Random.Range(0, 100);
        if (random < 50)
        {
            GameObject instantiatedDefensebuff = Instantiate(DefenseBuffGameObject, transform);
            StatBuff defenseBuff = instantiatedDefensebuff.GetComponent<StatBuff>();
            defenseBuff.BuffName = "견고함+";
            defenseBuff.BuffDurationTurns = 3;
            defenseBuff.IsAlwaysApplyBuff = true;
            defenseBuff.BuffStackType = BuffStackType.ResetDuration;
            defenseBuff.changeStat.SetValue(StatType.Defense, 5);
            SkillOwner.ApplyBuff(SkillOwner,SkillOwner,defenseBuff);
        }
    }
    
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        Stat finalStat = SkillOwner.FinalStat;
        float FinalDamage = 4 + finalStat.GetValue(StatType.Defense) * 20 / 100;
        if(isCrit) FinalDamage *= 2;
        return FinalDamage;
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "쇠바람+\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "단일 대상에게 4 + " + finalStat.GetValue(StatType.Defense) * 20 / 100 + "의 방어 기반 고정 피해를 주고\n" +
                    "50%의 확률로 우치에게 3턴동안 방어력 5만큼 부여";
    }
    
    public override void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
        if (SkillOwner == null)
        {
            SkillOwner = BattleManager.GetInstance.Allies.GetWoochi();
        }
        MainCharacterSkillSO mainCharacterSkillSo = SkillSO as MainCharacterSkillSO;
        skillDescription.text = "도력 " + mainCharacterSkillSo.RequiredSorceryPoints + "을 소모\n" +
                                "단일 대상에게 4 + 방어*20% 피해\n" +
                                "50%의 확률로 우치에게\n" +
                                "3턴동안 방어 5 부여";
    }
}