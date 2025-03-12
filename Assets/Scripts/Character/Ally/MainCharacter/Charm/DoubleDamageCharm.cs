using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoubleDamageCharm : BaseCharm
{
    public override void Activate(BaseCharacter opponent)
    {
        if(opponent == null || opponent.IsDead) return;

        BaseCharm charmObject = Instantiate(this);
        DoubleDamageBuff buff = charmObject.GetComponent<DoubleDamageBuff>();
        buff.BuffName = CharmName;
        buff.BuffDurationTurns = 1;
        BaseCharacter caster = BattleManager.GetInstance.Allies.GetWoochi();
        
        opponent.ApplyBuff( caster,opponent, buff);
    }

    public override void SetCharmDescription(TextMeshProUGUI text)
    {
        base.SetCharmDescription(text);
        string description = text.text;
        description += "1턴 동안 자신 및 아군 1명의 피해 * 2";

        text.text = description;
    }
}
