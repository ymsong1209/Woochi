using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CleanseSingleDebuffCharm : BaseCharm
{

    protected override void SetCharmDescription(TextMeshProUGUI text)
    {
        base.SetCharmDescription(text);
        text.text += "자신 및 아군에게 걸린 디버프 중 하나를 랜덤하게 제거";
    }
    public override void Activate(BaseCharacter opponent)
    {
        foreach(BaseBuff buff in opponent.activeBuffs)
        {
            if(buff.BuffType == BuffType.Negative && buff.IsRemovableDuringBattle)
            {
                Logger.BattleLog($"\"{opponent.Name}\"({opponent.RowOrder + 1}열)의 \"{buff.BuffName}\" 제거", "정화 부적");
                opponent.RemoveBuff(buff);
                break;
            }
        }
    }
}
