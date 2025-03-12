using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealCharm : BaseCharm
{
   
    public override void SetCharmDescription(TextMeshProUGUI text)
    {
        base.SetCharmDescription(text);
        text.text += "아군 전체의 체력 50%만큼 회복";
    }
    
    public override void Activate(BaseCharacter opponent)
    {
        opponent.Health.Heal(opponent.Health.MaxHealth/2);
    }
}