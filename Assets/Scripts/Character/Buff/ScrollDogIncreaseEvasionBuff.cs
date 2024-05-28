using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollDogIncreaseEvasionBuff : BaseBuff
{
    [SerializeField] private GameObject evasionBuffGameObject;
    //회피 스택 중첩 수
    [SerializeField,ReadOnly] private int evasionStack = 0;
    
    public override int ApplyTurnEndBuff()
    {
        //회피 스택이 10번 이하이면, 회피 버프 추가.
        if (evasionStack < 10)
        {
            GameObject instantiatedevasionbuff = Instantiate(evasionBuffGameObject, transform);
            StatBuff evasionBuff = instantiatedevasionbuff.GetComponent<StatBuff>();
            evasionBuff.StatBuffName = "족자";
            evasionBuff.BuffDurationTurns = -1;
            evasionBuff.ChanceToApplyBuff = 100;
            evasionBuff.ChangeEvasion = 2;
            buffOwner.ApplyBuff(buffOwner,evasionBuff);
        }
        if (buffOwner.CheckDead()) return -1;
        return 0;
    }

    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "족자" + ": 자신의 턴이 종료될 때마다 회피가 2씩 상승한다.\n";
        description += "(최대 10번까지 중첩 가능)\n";
        description += "현재 중첩 수 : " + evasionStack + "\n";
        text.text = description;
        text.color = Color.red;
    }
}
