using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Haetae_HolyWater_P : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        GameObject elementalDebuffPrefab = BuffPrefabList[0];
        GameObject elementalDebuffGameObject = Instantiate(elementalDebuffPrefab, transform);
        ElementalStatDeBuff elementalDebuff = elementalDebuffGameObject.GetComponent<ElementalStatDeBuff>();
        elementalDebuff.BuffName = "억제의 물+";
        elementalDebuff.BuffDurationTurns = 2;
        elementalDebuff.IsAlwaysApplyBuff = true;
        elementalDebuff.Element = SkillElement.Fire;
        elementalDebuff.ChangeStat = -5;
        instantiatedBuffList.Add(elementalDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
    }
    
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "역치+\n" + 
                    "대상 전체에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" + 
                    "2턴동안 불속성 공격력 -5만큼 부여";
    }
}
