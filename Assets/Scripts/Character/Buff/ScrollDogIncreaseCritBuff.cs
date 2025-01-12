using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 족자구 전용 버프. 매 턴마다 치명 2씩 증가시킨다.
/// </summary>
public class ScrollDogIncreaseCritBuff : BaseBuff
{
    [SerializeField] private GameObject statBuffGameObject;
    //회피 스택 중첩 수
    [SerializeField,ReadOnly] private int critStack = 0;
    
    public ScrollDogIncreaseCritBuff()
    {
        buffEffect = BuffEffect.IncreaseCritOvertime;
        buffType = BuffType.Positive;
    }
    
    public override int ApplyTurnEndBuff()
    {
        base.ApplyTurnEndBuff();
        //치명 스택이 10번 이하이면, 치명 버프 추가.
        if (critStack < 10)
        {
            GameObject instantiatedcritbuff = Instantiate(statBuffGameObject, transform);
            StatBuff critBuff = instantiatedcritbuff.GetComponent<StatBuff>();
            critBuff.BuffName = "맹공";
            critBuff.BuffDurationTurns = -1;
            critBuff.ChanceToApplyBuff = 100;
            critBuff.IsAlwaysApplyBuff = true;
            critBuff.BuffStackType = BuffStackType.StackEffect;
            critBuff.changeStat.SetValue(StatType.Crit, 2);
            buffOwner.ApplyBuff(buffOwner,buffOwner,critBuff);
            critStack++;
        }
        return 0;
    }

    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "맹공 : 자신의 턴이 종료될 때마다 치명이 2씩 상승한다.\n";
        description += "(최대 10번까지 중첩 가능)\n";
        description += "현재 중첩 수 : " + critStack + "\n";
        text.text = description;
        SetBuffColor(text);
    }
}
