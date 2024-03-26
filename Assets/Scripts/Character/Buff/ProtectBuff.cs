using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ProtectBuff : BaseBuff
{

    [SerializeField] private BaseCharacter protectionOwner;//누구에게 보호받는지

    #region Getter Setter
    public BaseCharacter ProtectionOwner => protectionOwner;
    #endregion


}