using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fascinate_Rework_P : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        bool isStunResist = false;
        BaseBuff hollimbuff = null;
        foreach (BaseBuff activebuff in _Opponent.activeBuffs)
        {
            if(activebuff.BuffName == "홀림")
            {
                hollimbuff = activebuff;
            }
            if(activebuff.BuffEffect == BuffEffect.StunResist)
            {
                isStunResist = true;
            }
        }

        //홀림버프가 존재하고 적이 기절 저항 버프를 안 가지고 있을 경우
        //홀림버프 삭제 후, 기절 버프 부여
        if (hollimbuff !=null && !isStunResist)
        {
            base.ActivateSkill(_Opponent);
            
            if (SkillResult.IsAnyHit())
            {
                GameObject stunDebuffPrefab = BuffPrefabList[1];
                GameObject stunDebuffGameObject = Instantiate(stunDebuffPrefab, transform);
                StunDeBuff stunDebuff = stunDebuffGameObject.GetComponent<StunDeBuff>();
                stunDebuff.BuffDurationTurns = 1;
                stunDebuff.IsAlwaysApplyBuff = true;
                _Opponent.ApplyBuff(SkillOwner,_Opponent, stunDebuff);
                _Opponent.RemoveBuff(hollimbuff);
            }
            return;
        }
        
        //홀림이 없을 경우 홀림 부여
        if (hollimbuff == null)
        {
            GameObject statDebuffPrefab = BuffPrefabList[0];
            GameObject statDebuffGameObject = Instantiate(statDebuffPrefab, transform);
            StatDeBuff statDebuff = statDebuffGameObject.GetComponent<StatDeBuff>();
            statDebuff.BuffName = "홀림";
            statDebuff.BuffDurationTurns = 2;
            statDebuff.ChanceToApplyBuff = 100;
            statDebuff.BuffStackType = BuffStackType.ResetDuration;
            statDebuff.changeStat.SetValue(StatType.Accuracy, -2);
            statDebuff.changeStat.SetValue(StatType.Speed, -2);
            instantiatedBuffList.Add(statDebuffGameObject);
        }
        base.ActivateSkill(_Opponent);
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        Stat finalStat = SkillOwner.FinalStat;
        int minStat = (int)Mathf.Round(finalStat.GetValue(StatType.MinDamage) * SkillSO.BaseMultiplier / 100f);
        int maxStat = (int)Mathf.Round(finalStat.GetValue(StatType.MaxDamage) * SkillSO.BaseMultiplier / 100f);
        text.text = "홀리기+\n" + 
                    "대상에게 " + minStat + " ~ " + maxStat + "의 피해를 주고\n" +
                    "100% 확률로 2턴동안 홀림 부여\n" +
                    "홀림이 걸려있을 경우 100% 확률로 기절로 변경";
    }
}
