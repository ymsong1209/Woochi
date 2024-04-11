using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class BaseCharacter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
                                public CharacterStatSO characterStat;
    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS
    #region Character Stats
    [SerializeField]            private Health  health;
    [SerializeField]            private int     size = 1;
    [SerializeField,ReadOnly]   private float   speed;
    [SerializeField,ReadOnly]   private float   defense;
    [SerializeField,ReadOnly]   private float   crit;
    [SerializeField,ReadOnly]   private float   accuracy;
    [SerializeField,ReadOnly]   private float   evasion;
    [SerializeField,ReadOnly]   private float   resist;
    [SerializeField,ReadOnly]   private float   minStat;
    [SerializeField,ReadOnly]   private float   maxStat;
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
    public   List<BaseBuff>   activeBuffs = new List<BaseBuff>();
    public   List<BaseSkill>  activeSkills = new List<BaseSkill>();
   
    /// <summary>
    /// chatacterStat�� �ִ� skillSO�� ��� BaseSkill�� ���� ���� ���ϴ� bool
    /// ũ��� characterStat ������ skills�� ���̶� �����ؾ���.
    /// </summary>
    [SerializeField] private  List<bool> activeSkillCheckBox = new List<bool>();
    private List<BaseSkill>   totalSkills = new List<BaseSkill>();

    [SerializeField] private bool isAlly;
    private bool isTurnUsed; //�� ���� ������ �ڽ��� ���� ������� ���

    #endregion BATTLE STATS


    public void CheckSkillsOnTurnStart()
    { 
        foreach(BaseSkill activeskill in activeSkills)
        {
            activeskill.CheckTurnStart();
        }
    }

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
            case BuffTiming.BattleStart:
                return ApplyBuffs(buff => buff.ApplyBattleStartBuff());
            case BuffTiming.RoundStart:
                return ApplyBuffs(buff => buff.ApplyRoundStartBuff());
            case BuffTiming.RoundEnd:
                return ApplyBuffs(buff => buff.ApplyRoundEndBuff());
            case BuffTiming.TurnStart:
                return ApplyBuffs(buff => buff.ApplyTurnStartBuff());
            case BuffTiming.BattleEnd:
                return ApplyBuffs(buff => buff.ApplyBattleEndBuff());
            default:
                throw new ArgumentOutOfRangeException(nameof(timing), $"Unsupported buff timing: {timing}");
        }
    }

    /// <summary>
    /// ���� ������,ĳ������ ���� ��ŵ�ǰų� ĳ���Ͱ� ����� ��� false ��ȯ
    /// </summary>
    private bool ApplyBuffs(Func<BaseBuff, bool> applyBuffMethod)
    {
        bool mightDead = false;

        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            //ĳ������ ���� ��ŵ�ǰų�, ĳ���Ͱ� ���� ��� mightDead�� true�� ����
            if (!applyBuffMethod(activeBuffs[i]))
            {
                mightDead = true;
            }

            if (ShouldRemoveBuff(activeBuffs[i]))
            {
                RemoveBuffAtIndex(i);
            }
        }
        if (mightDead)
        {
            //�׾��� ��� ���� ó�� �ϰ� ����
            if (CheckDead())
            {
                return HandleDeath();
            }
            //���� ��ŵ�� ��
            return false;
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
        minStat = characterStat.BaseMinStat;
        maxStat = characterStat.BaseMaxStat;
        isDead = false;
        health = GetComponent<Health>();
        health.MaxHealth = characterStat.BaseHealth;
        health.CurHealth = characterStat.BaseHealth;

        #region ��ų �ʱ�ȭ
        //activeSkills�� size��ŭ CharacterStat�� skill�� �տ������� �����ͼ� �����Ѵ�.
        for(int i = 0; i < activeSkills.Count; ++i)
        {
            if (characterStat.Skills[i] != null)
            {
                BaseSkill newSkill = Instantiate(characterStat.Skills[i], this.transform);
               
                newSkill.Initialize();
                newSkill.SkillOwner = this;
                if (activeSkillCheckBox[i])
                { 
                    activeSkills.Add(newSkill);
                }
                totalSkills.Add(newSkill);
            }
        }
        #endregion
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

    #region ���콺 �̺�Ʈ
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAlly)
            return;

        UIManager.GetInstance.SetEnemyToolTip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.enemyTooltip.SetActive(false);
    }
    #endregion
    #region Getter Setter
    public int Size => size;

    public Health Health => health;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public float Defense
    {
        get { return defense; }
        set { defense = value; }
    }
    public float Crit
    {
        get { return crit; }
        set { crit = value; }
    }
    public float Accuracy
    {
        get { return accuracy; }
        set { accuracy = value; }
    }
    public float Evasion
    {
        get { return evasion; }
        set { evasion = value; }
    }
    public float Resist
    {
        get { return resist; }
        set { resist = value; }
    }

    public float MinStat
    {
        get { return minStat; }
        set { minStat = value; }
    }

    public float MaxStat
    {
        get { return maxStat; }
        set { maxStat = value; }
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

    #region Validation
    private void OnValidate()
    {
        #region activeSkill Size Check

        
        if(activeSkills.Count > 4)
        {
            Debug.Log("ActiveSkill�� ������" + this.name.ToString() + "���� 4���� �ѽ��ϴ�.");
        }


        //CharacterStat�� Skill������ �ΰ� �� �� 4���� �̾ư� �� �����Ƿ� �ּ� ó��

        ////activeSkillCheckBox ũ��� CharacterStat�� Skill������ �����ؾ���
        //if (activeSkillCheckBox.Count != characterStat.Skills.Count)
        //{
        //    Debug.Log(nameof(activeSkillCheckBox) +"��" + nameof(characterStat.Skills) +"�� ����� " 
        //                    +this.name.ToString() +"���� �������� �ʽ��ϴ�." );
        //}
        #endregion activeSkill Size Check

        #region activeSkillCheckBox Count Check
        //activeSkillCheckBox true�� �Ȱ� 4���� �Ѿ�� �ȵ�
        int activeSkillsCount = 0;
        for(int i = 0; i < activeSkillCheckBox.Count; i++)
        {
            if (activeSkillCheckBox[i])
            {
                ++activeSkillsCount;
            }
        }
        if (activeSkillsCount > 4)
        {
            Debug.Log(this.name.ToString() + "������ " + nameof(activeSkillCheckBox) +
                    "���� Ȱ��ȭ�� ��ų ������ 4���� �ѽ��ϴ�.");
        }
        # endregion activeSkillCheckBox Count Check
    }

    #endregion
}
