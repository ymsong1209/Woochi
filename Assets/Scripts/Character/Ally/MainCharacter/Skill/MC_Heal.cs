using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_Heal : MainCharacterSkill
{
    [SerializeField]  int healAmount;
    
    protected virtual void ApplyStat(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.Health;
        //최소, 최대 대미지 사이의 수치를 고름

        int heal = healAmount;
        if (_isCrit) heal = heal * 2;
        opponentHealth.Heal((int)Mathf.Round(heal));
    }

    public MC_Heal()
    {
        requiredSorceryPoints = 100;
    }
}
