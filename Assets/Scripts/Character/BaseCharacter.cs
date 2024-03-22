using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class BaseCharacter : MonoBehaviour
{
    CharacterStatSO characterStat;
    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS
    #region Character Stats
    [SerializeField]            private Health  health;
    [SerializeField,ReadOnly]   private int     speed;
    [SerializeField,ReadOnly]   private int     defense;
    [SerializeField,ReadOnly]   private int     crit;
    [SerializeField,ReadOnly]   private int     accuracy;
    [SerializeField,ReadOnly]   private int     evasion;
    [SerializeField,ReadOnly]   private int     resist;
    #endregion


    #region Header BATTLE STATS

    [Space(10)]
    [Header("Battle Stats")]

    #endregion Header BATTLE STATS
    #region BATTLE STATS
    [SerializeField, ReadOnly] private bool             isDead;
    /// <summary>
    /// 나에게 적용된 버프
    /// </summary>
                               public  List<BaseBuff>   activeBuffs;
                               protected bool           isAlly;
    #endregion BATTLE STATS

    #region 버프 처리
    public void ApplyTurnStartBuffs()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].ApplyBuffWhenMyTurn();

            // 버프 지속 시간이 0이면 리스트에서 해당 버프 제거
            if (activeBuffs[i].BuffDurationTurns <= 0)
            {
                // 버프가 지워지면서 실행되어야할 함수들
                activeBuffs[i].RemoveBuff();

                // 리스트에서 해당 버프 제거
                activeBuffs.RemoveAt(i);
            }
        }
    }

    #endregion


    #region 기본 스탯 초기화
    /// <summary>
    /// 기본 스탯 초기화
    /// </summary>
    public virtual void Initialize()
    {
        speed = characterStat.BaseSpeed;
        defense = characterStat.BaseDefense;
        crit = characterStat.BaseCrit;
        accuracy = characterStat.BaseAccuracy;
        evasion = characterStat.BaseEvasion;
        resist = characterStat.BaseResist;
        isDead = false;
        health = GetComponent<Health>();
        health.MaxHealth = characterStat.BaseHealth;
        health.CurHealth = characterStat.BaseHealth;
    }
    #endregion 기본 스탯 초기화

    #region 죽음 처리
    /// <summary>
    /// Character가 죽었는지 확인
    /// </summary>
    public void CheckDead()
    {
        if(health.CurHealth <= 0)
        {
            SetDead(true);
        }
    }
    public virtual void SetDead(bool _dead)
    {
        isDead = _dead;
    }
    #endregion 죽음 처리

    #region Getter Setter
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }
    public int Crit
    {
        get { return crit; }
        set { crit = value; }
    }
    public int Accuracy
    {
        get { return accuracy; }
        set { accuracy = value; }
    }
    public int Evasion
    {
        get { return evasion; }
        set { evasion = value; }
    }
    public int Resist
    {
        get { return resist; }
        set { resist = value; }
    }

    public bool IsDead => isDead;

    public bool IsAlly => isAlly;
    #endregion

}
