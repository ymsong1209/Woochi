using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MC_Heal : MainCharacterSkill
{
    [SerializeField]  int healAmount;
    
    protected override int ApplyStat(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.Health;
        //최소, 최대 대미지 사이의 수치를 고름

        int heal = healAmount;
        if (_isCrit) heal = heal * 2;
        opponentHealth.Heal((int)Mathf.Round(heal));
        return heal;
    }

    public MC_Heal()
    {
        requiredSorceryPoints = 80;
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "여우비\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "아군 전체의 체력을 " + healAmount + "회복";
    }
}
