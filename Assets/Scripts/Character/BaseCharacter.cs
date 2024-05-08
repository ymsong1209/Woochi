using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(BaseCharacterHUD))]
[DisallowMultipleComponent]
public class BaseCharacter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterStatSO characterStat;
    [SerializeField] private Animator animator;
    [SerializeField] private BaseCharacterHUD characterHUD;

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
    [SerializeField] protected  List<bool> activeSkillCheckBox = new List<bool>();
    protected List<BaseSkill>   totalSkills = new List<BaseSkill>();

    [SerializeField,ReadOnly] protected bool isAlly;
    protected bool isTurnUsed = false; //한 라운드 내에서 자신의 턴을 사용했을 경우
    protected bool isIdle = true;

    public int rowOrder; // 캐릭터가 앞 열에서부터 몇 번째 순서인지
    #endregion BATTLE STATS


    public virtual void CheckSkillsOnTurnStart()
    { 
        // foreach(BaseSkill activeskill in activeSkills)
        // {
        //     activeskill.CheckTurnStart();
        // }
    }

    /// <summary>
    /// 몬스터 AI
    /// </summary>
    public virtual void TriggerAI()
    {
        
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

    /// <summary>
    /// 버프 적용후,캐릭터의 턴이 스킵되거나 캐릭터가 사망할 경우 false 반환
    /// </summary>
    private bool ApplyBuffs(Func<BaseBuff, bool> applyBuffMethod)
    {
        bool mightDead = false;

        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            //캐릭터의 턴이 스킵되거나, 캐릭터가 죽을 경우 mightDead를 true로 설정
            if (!applyBuffMethod(activeBuffs[i]))
            {
                mightDead = true;
            }

            if (ShouldRemoveBuff(activeBuffs[i]))
            {
                BaseBuff buff = activeBuffs[i];
                RemoveBuffAtIndex(i);
                Destroy(buff.gameObject);
            }
        }
        if (mightDead)
        {
            //죽었을 경우 버프 처리 하고 죽음
            if (CheckDead())
            {
                return HandleDeath();
            }
            //턴을 스킵만 함
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
            BaseBuff buff = activeBuffs[i];
            activeBuffs[i].RemoveBuff();
            activeBuffs.RemoveAt(i);
            Destroy(buff.gameObject);
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
        minStat = characterStat.BaseMinStat;
        maxStat = characterStat.BaseMaxStat;
        isDead = false;
        health = GetComponent<Health>();
        health.MaxHealth = characterStat.BaseHealth;
        health.CurHealth = characterStat.BaseHealth;
        #region 스킬 초기화
        //activeSkills의 size만큼 CharacterStat의 skill을 앞에서부터 가져와서 세팅한다.
        for(int i = 0; i < activeSkillCheckBox.Count; ++i)
        {
            BaseSkill newSkill = Instantiate(characterStat.Skills[i], this.transform);
            // // Instantiate된 스킬을 본래 타입으로 캐스팅
            // System.Type originalType = characterStat.Skills[i].GetType();
            // newSkill = (BaseSkill)gameObject.AddComponent(originalType);
            // CopyFields(characterStat.Skills[i], newSkill);
            
            newSkill.Initialize();
            newSkill.SkillOwner = this;
            if (activeSkillCheckBox[i])
            {
                activeSkills.Add(newSkill);
            }
            totalSkills.Add(newSkill);
        }


        #endregion
    }
    
    // 스킬의 모든 필드를 복사하는 메서드
    void CopyFields(object source, object target)
    {
        var sourceType = source.GetType();
        var targetType = target.GetType();

        foreach (var field in sourceType.GetFields(System.Reflection.BindingFlags.Public |
                                                   System.Reflection.BindingFlags.NonPublic |
                                                   System.Reflection.BindingFlags.Instance))
        {
            var targetField = targetType.GetField(field.Name, System.Reflection.BindingFlags.Public |
                                                              System.Reflection.BindingFlags.NonPublic |
                                                              System.Reflection.BindingFlags.Instance);
            if (targetField != null)
            {
                targetField.SetValue(target, field.GetValue(source));
            }
        }
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
            PlayAnimation(AnimationType.Dead);
            return true;
        }
        return false;
    }

    public virtual void SetDead()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    //캐릭터 완전 삭제
    public virtual void Destroy()
    {
        foreach(BaseBuff buff in activeBuffs)
        {
            Destroy(buff.gameObject);
        }
    }
    #endregion

    #region 마우스 이벤트
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

    #region 애니메이션
    public void PlayAnimation(AnimationType _type)
    {
        if(_type == AnimationType.Idle || isDead)
        {
            return;
        }

        animator.SetTrigger(_type.ToString());
        StartCoroutine(WaitAnim());
    }

    /// <summary>
    /// 현재 플레이 중인 애니메이션이 끝나기까지 기다림
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAnim()
    {
        isIdle = false;
        yield return null;

        while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        isIdle = true;
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

    public bool IsIdle => isIdle;
    #endregion

    #region Validation
    // private void OnValidate()
    // {
    //     #region activeSkillCheckBox Size Check
    //
    //     //activeSkillCheckBox 크기랑 CharacterStat의 Skill개수랑 동일해야함
    //     if (activeSkillCheckBox.Count != characterStat.Skills.Count)
    //     {
    //         Debug.Log(nameof(activeSkillCheckBox) + "랑" + nameof(characterStat.Skills) + "의 사이즈가 "
    //                         + this.name.ToString() + "에서 동일하지 않습니다.");
    //     }
    //     #endregion activeSkillCheckBox Size Check
    //
    //     #region activeSkillCheckBox Count Check
    //     //activeSkillCheckBox true로 된게 5개가 넘어가면 안됨
    //     int activeSkillsCount = 0;
    //     for(int i = 0; i < activeSkillCheckBox.Count; i++)
    //     {
    //         if (activeSkillCheckBox[i])
    //         {
    //             ++activeSkillsCount;
    //         }
    //     }
    //     if (activeSkillsCount > 5)
    //     {
    //         Debug.Log(this.name.ToString() + "에서의 " + nameof(activeSkillCheckBox) +
    //                 "에서 활성화된 스킬 개수가 5개가 넘습니다.");
    //     }
    //     # endregion activeSkillCheckBox Count Check
    // }

    #endregion
}
