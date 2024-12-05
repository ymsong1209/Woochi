using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_LeftDummy : BaseEnemy
{

    public override void TriggerAI()
    {
        Debug.Log("LeftDummyAI");
    }
    //초기 체력을 음수로 해서 IsDead활성화 후, 양수로 변경해서 formation에서 제거되지 않게 설정
    public override void Initialize()
    {
        base.Initialize();
        Health.CurHealth = 10;
    }
}
