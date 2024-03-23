using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseAllyCharacter : BaseCharacter
{



    public override void Initialize(){
        base.Initialize();
        base.isAlly = true;
    }


    #region Getter, Setter
    #endregion
}