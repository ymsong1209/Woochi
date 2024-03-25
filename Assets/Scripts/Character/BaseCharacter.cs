using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class BaseCharacter : MonoBehaviour
{
    [SerializeField] private CharacterStatSO characterStat;
    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS
    #region Character Stats
    [SerializeField]            private Health  health;
    [SerializeField]            private int     size = 1;
    [SerializeField,ReadOnly]   private int     speed;
    [SerializeField,ReadOnly]   private int     defense;
    [SerializeField,ReadOnly]   private int     crit;
    [SerializeField,ReadOnly]   private int     accuracy;
    [SerializeField,ReadOnly]   private int     evasion;
    [SerializeField,ReadOnly]   private int     resist;
    #endregion

    #region Header SpecializedStats
    [Tooltip("Ư�� ��ġ���� Spawn�ǰ� �ϰ� ������ �� �Է�.")]
    [SerializeField] private bool       isSpawnSpecific = false;
    [SerializeField] private Vector3    spawnLocation;
    [SerializeField] private Quaternion spawnRotation;
    #endregion

    #region Header BATTLE STATS

    [Space(10)]
    [Header("Battle Stats")]

    #endregion Header BATTLE STATS
    #region BATTLE STATS
    [SerializeField, ReadOnly] private bool      isDead;
    /// <summary>
    /// ������ ����� ����
    /// </summary>
    public   List<BaseBuff>   activeBuffs;
    public   List<BaseSkill>  skills;
    protected bool            isAlly;
    private bool isTurnUsed; //�� ���� ������ �ڽ��� ���� ������� ���

    #endregion BATTLE STATS



    #region ���� ó��
    /// <summary>
    /// ���� ���� ������ ���� ������ ���� ó�� �Լ� ȣ��
    /// ���� ȿ�� ��� �� ĳ���Ͱ� ��������� true��ȯ
    /// �׾��� ��쿣 ĳ���Ͱ� ������ �ִ� ��� ���� ���� �� ��� ó��
    /// </summary>
    public bool ApplyBuff(BuffTiming timing)
    {
        switch (timing)
        {
            case BuffTiming.RoundStart:
                return ApplyBuffs(buff => buff.ApplyRoundStartBuff());
            case BuffTiming.RoundEnd:
                return ApplyBuffs(buff => buff.ApplyRoundEndBuff());
            case BuffTiming.TurnStart:
                return ApplyBuffs(buff => buff.ApplyTurnStartBuff());
            default:
                throw new ArgumentOutOfRangeException(nameof(timing), $"Unsupported buff timing: {timing}");
        }
    }

    private bool ApplyBuffs(Func<BaseBuff, bool> applyBuffMethod)
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            if (!applyBuffMethod(activeBuffs[i]))
            {
                return HandleDeath();
            }

            if (ShouldRemoveBuff(activeBuffs[i]))
            {
                RemoveBuffAtIndex(i);
            }
        }
        return true;
    }

    private bool ShouldRemoveBuff(BaseBuff buff)
    {
        return buff.BuffDurationTurns <= 0;
    }

    private void RemoveBuffAtIndex(int index)
    {
        if (!activeBuffs[index].RemoveBuff())
        {
            // Buff�� ���ŵǸ鼭 ĳ���Ͱ� ����ϴ� ���� ���⼭ �ٷ��� ����
            // �� �Լ��� �ܼ��� ������ �����ϴ� ���Ҹ� ������
        }
        activeBuffs.RemoveAt(index);
    }

    private bool HandleDeath()
    {
        RemoveAllBuff();
        return false;
    }

    public void RemoveAllBuff()
    {
        //��� ���� ��ȸ
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].RemoveBuff();
            activeBuffs.RemoveAt(i);
        }
    }

    #endregion


    #region �⺻ ���� �ʱ�ȭ
    /// <summary>
    /// �⺻ ���� �ʱ�ȭ
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
    #endregion �⺻ ���� �ʱ�ȭ

    #region ���� ó��
    /// <summary>
    /// Character�� �׾����� Ȯ��
    /// </summary>
    public bool CheckDead()
    {
        if(health.CheckHealthZero())
        {
            SetDead(true);
            return true;
        }
        return false;
    }
    public virtual void SetDead(bool _dead)
    {
        isDead = _dead;
    }

    //ĳ���� ���� ����
    public virtual void Destroy()
    {
        foreach(BaseBuff buff in activeBuffs)
        {
            Destroy(buff);
        }
    }
    #endregion ���� ó��

    #region Getter Setter
    public int Size => size;

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
    public bool IsAlly
    {
        get { return isAlly; }
        set { isAlly = value; }
    }
    public bool IsSpawnSpecific => isSpawnSpecific;
    public Vector3 SpawnLocation => spawnLocation;
    public Quaternion SpawnRotation => spawnRotation;

    public bool IsTurnUsed
    {
        get { return isTurnUsed; }
        set { isTurnUsed = value; }
    }
    #endregion

}
