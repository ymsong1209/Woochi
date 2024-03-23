using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseEnemyCharacter : BaseCharacter
{

    public override void Initialize()
    {
        base.Initialize();
        base.isAlly = false;
    }


    #region Getter, Setter
    #endregion
}