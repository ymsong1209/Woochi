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
    public  List<BaseBuff>   activeBuffs;
    protected bool           isAlly;
    public  List<BaseSkill>  skills;
    #endregion BATTLE STATS



    #region ���� ó��
    public void ApplyTurnStartBuffs()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].ApplyBuffWhenMyTurn();

            // ���� ���� �ð��� 0�̸� ����Ʈ���� �ش� ���� ����
            if (activeBuffs[i].BuffDurationTurns <= 0)
            {
                // ������ �������鼭 ����Ǿ���� �Լ���
                activeBuffs[i].RemoveBuff();

                // ����Ʈ���� �ش� ���� ����
                activeBuffs.RemoveAt(i);
            }
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
    public void CheckDead()
    {
        if(health.CheckHealthZero())
        {
            SetDead(true);
        }
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
    public bool IsAlly => isAlly;
    public bool IsSpawnSpecific => isSpawnSpecific;
    public Vector3 SpawnLocation => spawnLocation;
    public Quaternion SpawnRotation => spawnRotation;
    #endregion

}
