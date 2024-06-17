using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Rip : BaseSkill
{
    protected override void ApplyStat(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.gameObject.GetComponent<Health>();
        //최소, 최대 대미지 사이의 수치를 고름

        float randomStat = Random.Range(SkillOwner.MinStat, SkillOwner.MaxStat);
        //피해량 계수를 곱함
        randomStat *= (Multiplier / 100);

        //방어 스탯을 뺌
        randomStat = randomStat * (100 - _opponent.Defense) / 100;

        //적에게 출혈 버프가 붙어있으면 1.5배의 대미지
        bool hasBleed = false;
        foreach(BaseBuff buff in _opponent.activeBuffs)
        {
            if(buff.BuffEffect == BuffEffect.Bleed)
            {
                hasBleed = true;
            }
        }
        if (hasBleed) randomStat *= 1.5f;

        //치명타일 경우 최종대미지가 2배
        if (_isCrit) randomStat *= 2;

        opponentHealth.ApplyDamage((int)Mathf.Round(randomStat));
    }
}
