using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseCharacter
{
    /// <summary>
    /// 몬스터 AI
    /// </summary>
    public virtual void TriggerAI()
    {
    }

    public override bool CheckUsableSkill() => true;
}
