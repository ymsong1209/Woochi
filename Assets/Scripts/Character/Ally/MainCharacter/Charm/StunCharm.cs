using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StunCharm : BaseCharm
{
    public override void SetCharmDescription(TextMeshProUGUI text)
    {
        base.SetCharmDescription(text);
        text.text += "모든 적에게 기절 부여";
    }
    
    public override void Activate(BaseCharacter opponent)
    {
        if(opponent == null || opponent.IsDead) return;

        BaseCharm charmObject = Instantiate(this);
        StunDeBuff buff = charmObject.GetComponent<StunDeBuff>();
        buff.BuffName = CharmName;
        buff.BuffDurationTurns = Turns;
        BaseCharacter caster = BattleManager.GetInstance.Allies.GetWoochi();
        
        foreach(BaseBuff basebuff in opponent.activeBuffs)
        {
            if (basebuff.CanApplyBuff(buff) == false)
            {
                Logger.BattleLog($"\"{CharmName}\"내부의 \"{buff.name}\" 버프 적용 실패 on \"{opponent.Name}\"({opponent.RowOrder + 1}열) because of Buff \"{basebuff.name}\"", "버프 적용 가능 여부");
                return;
            }
        }
        opponent.ApplyBuff( caster,opponent, buff);
    }
}
