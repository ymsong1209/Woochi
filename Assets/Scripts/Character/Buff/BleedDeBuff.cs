using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 3턴 동안 대상자는 매 턴 마다 최대 체력의 bleedpercent%의 데미지를 입는다.
/// 출혈 디버프가 있는 동안 다시 출혈에 걸리면 중첩이 올라가고
/// 최대 체력 * bleedpercent / 100 %의 데미지를 준다.
/// </summary>
public class BleedDeBuff : BaseBuff
{
  
    //출혈 중첩 수
    [SerializeField,ReadOnly] private int bleedPercent = 0;
    
    public BleedDeBuff()
    {
        buffEffect = BuffEffect.Bleed;
        buffType = BuffType.Negative;
        buffStackType = BuffStackType.ResetAndStack;
    }
    
    public override int ApplyTurnStartBuff()
    {
        Logger.BattleLog($"\"{buffOwner.Name}\"({buffOwner.RowOrder + 1}열)은 출혈 상태입니다\n"+
                         $"출혈 중첩 : {bleedPercent}, 남은 횟수 : {buffDurationTurns}", "출혈버프");
        //전체체력에서 bleedApply%만큼 피를 깎는다.
        float bleedDamage = buffOwner.Health.MaxHealth * bleedPercent / 100f;
        buffOwner.Health.ApplyDamage((int)Mathf.Round(bleedDamage));

        --buffDurationTurns;

        return (int)Mathf.Round(bleedDamage);
    }
    
    
    protected override void StackBuffEffect(BaseBuff _buff)
    {
        BleedDeBuff bleedDeBuff = _buff as BleedDeBuff;
        if (bleedDeBuff)
        {
            bleedPercent += bleedDeBuff.BleedPercent;
            Logger.BattleLog($"\"{buffOwner.Name}\"({buffOwner.RowOrder + 1}열)에게 출혈 중첩\n"+
                             $"출혈 중첩 : {bleedPercent}, 남은 횟수 : {buffDurationTurns}", "출혈버프");
        }
        else
        {
            Logger.BattleLog($"\"{buffOwner.Name}\"({buffOwner.RowOrder + 1}열)에게 출혈 중첩 실패\n"+
                             $"인자로 들어온 출혈버프가 변환 불가!", "출혈버프 실패");
        }
    }

    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "출혈" + buffDurationTurns+ "턴 : 매턴마다 최대 체력의 " + bleedPercent + "% 만큼 피해를 입습니다.";
        text.text = description;
        SetBuffColor(text);
    }
    
   
    
    public int BleedPercent
    {
        get
        {
        return bleedPercent;
        }
        set
        {
            bleedPercent = value;
        }
    }
}
