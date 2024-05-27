using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Bite : BaseSkill
{

    protected override void ApplyStat(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.gameObject.GetComponent<Health>();
        //최소, 최대 대미지 사이의 수치를 고름

        float RandomStat = Random.Range(SkillOwner.MinStat, SkillOwner.MaxStat);
        //피해량 계수를 곱함
        RandomStat *= (Multiplier / 100);

        //방어 스탯을 뺀 base 스탯을 구함
        RandomStat = RandomStat * (100 - _opponent.Defense) / 100;
        if (_isCrit) RandomStat = RandomStat * 2;

        // 물어뜯기로 피해를 입히면 적의 잃은 체력의 20% 만큼 추가 피해를 줌
        RandomStat +=  (opponentHealth.MaxHealth - opponentHealth.CurHealth) * 0.2f;

        //적에게 최종적인 대미지를 줌
        opponentHealth.ApplyDamageWithAnimation((int)Mathf.Round(RandomStat));

        //호랑이는 준 피해의 30%만큼 회복함.
        int healamount = (int)Mathf.Round(RandomStat * 0.3f);
        SkillOwner.Health.Heal(healamount);
    }
}
