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
    [SerializeField] private CharacterStatSO characterStat;
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
    #endregion

    #region Header SpecializedStats
    [Tooltip("특정 위치에서 Spawn되게 하고 싶으면 값 입력.")]
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
    /// 나에게 적용된 버프
    /// </summary>
    public   List<BaseBuff>   activeBuffs = new List<BaseBuff>();
    public   List<BaseSkill>  activeSkills = new List<BaseSkill>();
   
    /// <summary>
    /// chatacterStat에 있는 skillSO중 어떤걸 BaseSkill로 넣을 건지 정하는 bool
    /// 크기는 characterStat 내부의 skills의 길이랑 동일해야함.
    /// </summary>
    [SerializeField] private  List<bool> activeSkillCheckBox = new List<bool>();
    private List<BaseSkill>   totalSkills = new List<BaseSkill>();

    [SerializeField] private bool isAlly;
    private bool isTurnUsed; //한 라운드 내에서 자신의 턴을 사용했을 경우

    #endregion BATTLE STATS


    public void CheckSkillsOnTurnStart()
    { 
        foreach(BaseSkill activeskill in activeSkills)
        {
            activeskill.CheckTurnStart();
        }
    }

    #region 버프 처리
    /// <summary>
    /// 버프 적용 시점에 따라 적절한 버프 처리 함수 호출
    /// 버프 효과 사용 후 캐릭터가 살아있으면 true반환
    /// 죽었을 경우엔 캐릭터가 가지고 있는 모든 버프 제거 후 사망 처리
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
            // Buff가 제거되면서 캐릭터가 사망하는 경우는 여기서 다루지 않음
            // 이 함수는 단순히 버프를 제거하는 역할만 수행함
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
        //모든 버프 순회
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].RemoveBuff();
            activeBuffs.RemoveAt(i);
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

        #region 스킬 초기화
        for(int i = 0; i < characterStat.Skills.Count; ++i)
        {
            if (characterStat.Skills[i])
            {
                BaseSkill newSkill = new BaseSkill();
                newSkill.Initialize(characterStat.Skills[i]);
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
    #endregion 기본 스탯 초기화

    #region 죽음 처리
    /// <summary>
    /// Character가 죽었는지 확인
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

    //캐릭터 완전 삭제
    public virtual void Destroy()
    {
        foreach(BaseBuff buff in activeBuffs)
        {
            Destroy(buff);
        }
    }
    #endregion 죽음 처리

    #region 마우스 이벤트
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Onpointerentercalled");
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
        #region activeSkillCheckBox Size Check
        //activeSkillCheckBox 크기랑 CharacterStat의 Skill개수랑 동일해야함
        if (activeSkillCheckBox.Count != characterStat.Skills.Count)
        {
            Debug.Log(nameof(activeSkillCheckBox) +"랑" + nameof(characterStat.Skills) +"의 사이즈가 " 
                            +this.name.ToString() +"에서 동일하지 않습니다." );
        }
        #endregion activeSkillCheckBox Size Check

        #region activeSkillCheckBox Count Check
        //activeSkillCheckBox true로 된게 4개가 넘어가면 안됨
        int activeSkills = 0;
        for(int i = 0; i < activeSkillCheckBox.Count; i++)
        {
            if (activeSkillCheckBox[i])
            {
                ++activeSkills;
            }
        }
        if (activeSkills > 4)
        {
            Debug.Log(this.name.ToString() + "에서의 " + nameof(activeSkillCheckBox) +
                    "에서 활성화된 스킬 개수가 4개가 넘습니다.");
        }
        # endregion activeSkillCheckBox Count Check
    }

    #endregion
}
