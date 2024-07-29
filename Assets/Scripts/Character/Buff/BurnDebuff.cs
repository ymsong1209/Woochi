using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BurnDebuff : BaseBuff
{
    private float burnDamage;
    
    public override int ApplyTurnStartBuff()
    {
        //전체체력에서 burnDamage%만큼 대미지를 줌
        float burnAmount = buffOwner.Health.MaxHealth * burnDamage / 100f;
        buffOwner.Health.ApplyDamage((int)Mathf.Round(burnAmount));

        --buffDurationTurns;

        Debug.Log(buffOwner.name + "is Burning. Burn leftover turn : " + buffDurationTurns.ToString());

        //checkdead는 캐릭터가 죽었을경우 true 반환
        //ApplyTurnStartBuff는 버프 실행 후 캐릭터가 살았으면 true 반환
        return (int)Mathf.Round(burnAmount);
    }

    //화상 스택이 쌓일 경우 지속 시간이 3턴만큼 늘어난다.
    public override void StackBuff(BaseBuff _buff)
    {
        base.buffDurationTurns += 3;
    }
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        float burnAmount = buffOwner.Health.MaxHealth * burnDamage / 100f;
        string description = "화상" + buffDurationTurns+ " : 매턴마다"+ burnAmount + "만큼 피해를 입습니다.";
        text.text = description;
        SetBuffColor(text);
    }
    
    public BurnDebuff()
    {
        buffEffect = BuffEffect.Burn;
        buffType = BuffType.Negative;
        burnDamage = 5f;
    }
    
}
