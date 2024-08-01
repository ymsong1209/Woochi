using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TrollBuff : BaseBuff
{
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "공격이 빗나갈 경우 공격을 한번 더 합니다.";
        text.text += description;
        SetBuffColor(text);
    }

    public TrollBuff()
    {
        buffType = BuffType.Positive;
        buffEffect = BuffEffect.Troll;
    }
}
