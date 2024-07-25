using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_TF_SuperStrength : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0에서 99까지의 값을 랜덤으로 생성
        if (randomValue < 75) //75%의 확률로 적을 1열 뒤로 밈
        {
            BattleManager.GetInstance.MoveCharacter(_Opponent, -1);
        }
    }
}
