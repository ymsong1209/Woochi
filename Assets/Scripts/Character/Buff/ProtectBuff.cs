using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ProtectBuff : BaseBuff
{

    [SerializeField] private BaseCharacter protectionOwner;//�������� ��ȣ�޴���

    #region Getter Setter
    public BaseCharacter ProtectionOwner => protectionOwner;
    #endregion


}