using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    CharacterStatSO characterStat;
    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS

    [SerializeField,ReadOnly] private int Speed;
    [SerializeField,ReadOnly] private int Defense;
    [SerializeField,ReadOnly] private int Crit;
    [SerializeField,ReadOnly] private int Accuracy;
    [SerializeField,ReadOnly] private int Evasion;
    [SerializeField,ReadOnly] private int Resist;



    /// <summary>
    /// 기본 스탯 초기화
    /// </summary>
    public void Initialize()
    {
        Speed = characterStat.BaseSpeed;
        Defense = characterStat.BaseDefense;
        Crit = characterStat.BaseCrit;
        Accuracy = characterStat.BaseAccuracy;
        Evasion = characterStat.BaseEvasion;
        Resist = characterStat.BaseResist;
    }

}
