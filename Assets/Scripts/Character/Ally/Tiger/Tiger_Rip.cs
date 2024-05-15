using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Rip : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        //아군 보호 스킬등으로 보호 할 수 있음
        //최종적으로 공격해야하는 적 판정
        BaseCharacter opponent = CheckOpponentValid(_Opponent);

        if (opponent == null)
        {
            Debug.LogError("opponent is null");
            return;
        }

        //명중 체크
        if (CheckAccuracy() == false)
        {
            Debug.Log("Accuracy Failed on" + _Opponent.name.ToString());
            return;
        }
        //회피 체크
        if (CheckEvasion(opponent) == false)
        {
            Debug.Log(_Opponent.name.ToString() + "Evaded skill" + Name);
            return;
        }

        //치명타일 경우 바로 버프 적용
        if (CheckCrit())
        {
            Debug.Log("Crit Skill on " + Name + "to " + _Opponent.name.ToString());
            ApplyDamage(opponent, true);

            //버프 적용
            foreach (GameObject ApplybuffGameobject in bufflist)
            {
                if (ApplybuffGameobject == null) continue;
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                if (BufftoApply == null) continue;
                //먼저 buff/debuff가 몇%의 확률로 걸리는지 판단.
                if (CheckApplyBuff(BufftoApply) == false) continue;
                //치명타면 저항 무시한채 스킬 적용
                ApplyBuff(opponent, BufftoApply);
            }
        }
        else
        {
            Debug.Log("Non Crit Skill on " + Name + "to " + _Opponent.name.ToString());
            ApplyDamage(opponent, false);

            foreach (GameObject ApplybuffGameobject in bufflist)
            {
                if (ApplybuffGameobject == null) continue;
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                //먼저 buff/debuff가 몇%의 확률로 걸리는지 판단.
                if (CheckApplyBuff(BufftoApply) == false) continue;
                //적의 저항 수치 판단.
                if (CheckResist(opponent) == false)
                {
                    ApplyBuff(opponent, BufftoApply);
                }

            }
        }

    }

    private void ApplyDamage(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.gameObject.GetComponent<Health>();
        //최소, 최대 대미지 사이의 수치를 고름

        float RandomStat = Random.Range(SkillOwner.MinStat, SkillOwner.MaxStat);
        //피해량 계수를 곱함
        RandomStat *= (Multiplier / 100);

        //방어 스탯을 뺌
        RandomStat = RandomStat * (100 - _opponent.Defense) / 100;

        //적에게 출혈 버프가 붙어있으면 1.5배의 대미지
        bool hasBleed = false;
        foreach(BaseBuff buff in _opponent.activeBuffs)
        {
            if(buff.BuffType == BuffType.Bleed)
            {
                hasBleed = true;
            }
        }
        if (hasBleed) RandomStat = RandomStat * 1.5f;

        //치명타일 경우 최종대미지가 2배
        if (_isCrit) RandomStat = RandomStat * 2;

        opponentHealth.ApplyDamageWithAnimation((int)Mathf.Round(RandomStat));
    }
}
