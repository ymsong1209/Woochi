using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(BaseCharacterHUD))]
[RequireComponent(typeof(BaseCharacterCollider))]
[DisallowMultipleComponent]
public class BaseCharacter : MonoBehaviour
{
    [HideInInspector] public BaseCharacterHUD HUD;
    [HideInInspector] public BaseCharacterAnimation anim;
    [HideInInspector] public BaseCharacterCollider collider;
    
    [SerializeField]  protected CharacterStatSO characterStat;
    private BuffList buffList;
    
    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS
    #region Character Stats
    [SerializeField]            private Health  health;
    private SpriteRenderer sprite;   
    [SerializeField]            private Stat    stat;
    #endregion

   
    [Tooltip("특정 위치에서 Spawn되게 하고 싶으면 값 입력.")]
    [SerializeField] bool isMainCharacter = false;
    
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

    [SerializeField,ReadOnly] protected bool isAlly;
    protected bool isTurnUsed = false; //한 라운드 내에서 자신의 턴을 사용했을 경우
    protected bool isIdle = true;

    public bool isDummy = false;      // 더미 캐릭터인지
    public bool isStarting = false;     // 캐릭터가 전투 시작시 소환될건지
    [HideInInspector] public bool isSummoned = false;     // 캐릭터가 소환되었는지

    // 캐릭터가 앞 열에서부터 몇 번째 순서인지
    [SerializeField, ReadOnly] private int rowOrder;

    private bool isSelected = false;     // 배틀 중 스킬 대상으로 선택된 캐릭터인지
    #endregion BATTLE STATS

    #region Event
    public Action onHealthChanged;
    public Action<AnimationType> onPlayAnimation;
    public Action<AttackResult, int, bool> onAttacked;
    #endregion

    private void Awake()
    {
        HUD = GetComponent<BaseCharacterHUD>();
        anim = GetComponent<BaseCharacterAnimation>();
        collider = GetComponent<BaseCharacterCollider>();
        buffList = GetComponentInChildren<BuffList>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        isSummoned = isStarting;
    }

    public virtual void CheckSkillsOnTurnStart()
    { 
    }

    /// <summary>
    /// skillresult에 따라서 추가 행동이 필요한 경우, false를 반환한다.
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckTurnEndFromSkillResult(SkillResult result)
    {
        return true;
    }

    public void OnSelected()
    {
        isSelected = !isSelected;
        HUD.Selected(isSelected);
        BattleManager.GetInstance.CharacterSelected(this);
    }

    public void InitSelect()
    {
        isSelected = false;
        HUD.Selected(isSelected);
    }
    #region 버프 처리
    /// <summary>
    /// 버프 적용 시점에 따라 적절한 버프 처리 함수 호출
    /// 버프 효과 사용 후 캐릭터가 살아있으면 true반환
    /// 죽었을 경우엔 캐릭터가 가지고 있는 모든 버프 제거 후 사망 처리
    /// </summary>
    public bool TriggerBuff(BuffTiming timing)
    {
        switch (timing)
        {
            case BuffTiming.BattleStart:
                return TriggerBuffs(buff => buff.ApplyBattleStartBuff());
            case BuffTiming.RoundStart:
                return TriggerBuffs(buff => buff.ApplyRoundStartBuff());
            case BuffTiming.RoundEnd:
                return TriggerBuffs(buff => buff.ApplyRoundEndBuff());
            case BuffTiming.TurnStart:
                return TriggerBuffs(buff => buff.ApplyTurnStartBuff());
            case BuffTiming.TurnEnd:
                return TriggerBuffs(buff => buff.ApplyTurnEndBuff());
            case BuffTiming.BattleEnd:
                return TriggerBuffs(buff => buff.ApplyBattleEndBuff());
            default:
                throw new ArgumentOutOfRangeException(nameof(timing), $"Unsupported buff timing: {timing}");
        }
    }

    /// <summary>
    /// 버프 적용후,캐릭터의 턴이 스킵되거나 캐릭터가 사망할 경우 false 반환
    /// </summary>
    private bool TriggerBuffs(Func<BaseBuff, int> applyBuffMethod)
    {
        bool turnSkipped = false;

        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            //캐릭터의 턴이 스킵되거나, 캐릭터가 죽을 경우 turnSkipped를 true로 설정
            //버프를 통해 받은 대미지 반환. 죽었거나 기절한 경우 -1 반환
            int result = applyBuffMethod(activeBuffs[i]);
            if (result == -1)
            {
                turnSkipped = true;
            }
           

            if (ShouldRemoveBuff(activeBuffs[i]))
            {
                BaseBuff buff = activeBuffs[i];
                RemoveBuffAtIndex(i);
            }
        }
        
        //죽었을 경우 버프 처리 하고 죽음
        if (CheckDeadAndPlayAnim() || turnSkipped)
        {
            return false;
        }
       
        return true;
    }
    
    /// <summary>
    /// buff gameobject는 instantiated되어서 opponent에 붙어있음.
    /// </summary>
    /// <returns></returns>
    public virtual BaseBuff ApplyBuff(BaseCharacter caster, BaseCharacter receiver, BaseBuff _buff)
    {

        BaseBuff activeBuff = receiver.FindMatchingBuff(_buff);

        if (activeBuff)
        {
            // 기존 버프와 중첩
            activeBuff.StackBuff(_buff);
            return activeBuff;
        }

        // 새 버프 추가
        BaseBuff new_buff = buffList.TransferBuffAtIcon(receiver, _buff);
        new_buff.AddBuff(caster, receiver);
        return new_buff;
    }
    

    private bool ShouldRemoveBuff(BaseBuff buff)
    {
        if (buff.BuffDurationTurns < -1)
        {
            Debug.LogError(buff.name + "의 지속시간이 -1보다 작음");
        }
        return buff.BuffDurationTurns == 0;
    }

    public void RemoveBuffAtIndex(int index)
    {
        BaseBuff removebuff = activeBuffs[index];
        if (removebuff)
        {
            activeBuffs.RemoveAt(index);
            removebuff.RemoveBuff();
        }
    }

    public void RemoveBuff(BaseBuff bufftoremove)
    {
        if (activeBuffs.Remove(bufftoremove))
        {
            bufftoremove.RemoveBuff();
        }
    }

    private void HandleDeath()
    {
        RemoveAllBuff();
        anim.PlayDeadAnimation();
    }

    public void RemoveAllBuff()
    {
        //모든 버프 순회
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            BaseBuff buff = activeBuffs[i];
            activeBuffs.RemoveAt(i);
            buff.RemoveBuff();
        }
    }

    //stat buff가 적용되거나 사라지면 자신이 가진 모든 버프 순회해서 stat buff에 있는 스탯 적용
    public void CheckForStatChange()
    {
        //기존 스탯 다시 초기화
        stat = characterStat.BaseStat;
        
        foreach (BaseBuff buff in activeBuffs)
        {
            if (buff.BuffEffect == BuffEffect.StatStrengthen)
            {
                StatBuff statBuff = buff as StatBuff;
                stat += statBuff.ChangeStat;
            }
            else if (buff.BuffEffect == BuffEffect.StatWeaken)
            {
                StatDeBuff debuff = buff as StatDeBuff;
                stat += debuff.ChangeStat;
            }
        }
    }
    
    /// <summary>
    /// activebuffs에서 _buff와 같은 버프를 찾아 반환
    /// </summary>
    public BaseBuff FindMatchingBuff(BaseBuff _buff)
    {
        foreach (BaseBuff activeBuff in activeBuffs)
        {
            if (activeBuff == null || activeBuff.BuffEffect != _buff.BuffEffect) continue;

            if (_buff.BuffEffect == BuffEffect.StatStrengthen || _buff.BuffEffect == BuffEffect.StatWeaken || _buff.BuffEffect == BuffEffect.DotCureByDamage)
            {
                if (IsMatchingStatBuff(activeBuff, _buff) || IsMatchingStatDeBuff(activeBuff, _buff) || IsMatchingDotCureByDamageBuff(activeBuff, _buff))
                {
                    return activeBuff;
                }
            }
            else
            {
                return activeBuff;
            }
        }

        return null;
    }
    
    private bool IsMatchingStatBuff(BaseBuff activeBuff, BaseBuff _buff)
    {
        StatBuff activeStatBuff = activeBuff as StatBuff;
        StatBuff statBuff = _buff as StatBuff;
        return activeStatBuff != null && statBuff != null && activeStatBuff.StatBuffName == statBuff.StatBuffName;
    }

    private bool IsMatchingStatDeBuff(BaseBuff activeBuff, BaseBuff _buff)
    {
        StatDeBuff activeStatDebuff = activeBuff as StatDeBuff;
        StatDeBuff statDebuff = _buff as StatDeBuff;
        return activeStatDebuff != null && statDebuff != null && activeStatDebuff.StatBuffName == statDebuff.StatBuffName;
    }

    private bool IsMatchingDotCureByDamageBuff(BaseBuff activeBuff, BaseBuff _buff)
    {
        DotCureByDamageBuff activeDotHealBuff = activeBuff as DotCureByDamageBuff;
        DotCureByDamageBuff dotHealBuff = _buff as DotCureByDamageBuff;
        return activeDotHealBuff != null && dotHealBuff != null && activeDotHealBuff.BuffName == dotHealBuff.BuffName;
    }

    #endregion


    #region 기본 스탯 초기화
    public void InitializeStatSO() => characterStat.Initialize();

    /// <summary>
    /// 기본 스탯 초기화
    /// </summary>
    public virtual void Initialize()
    {
        InitializeStat();
        InitializeHealth();
        InitializeSkill();
        isDead = false;
    }
    
    protected void InitializeStat()
    {
        stat = characterStat.BaseStat;
    }

    protected void InitializeHealth()
    {
        health = GetComponent<Health>();
        health.MaxHealth = stat.health;
        health.CurHealth = stat.health;
    }

    protected virtual void InitializeSkill()
    {
        DestroyActiveSkills();
        //activeSkills의 size만큼 CharacterStat의 skill을 앞에서부터 가져와서 세팅한다.
        for(int i = 0; i < activeSkillCheckBox.Count; ++i)
        {
            if (activeSkillCheckBox[i])
            {
                InstantiateSkill(characterStat.Skills[i]);
            }
        }
    }

    protected void DestroyActiveSkills()
    {
        for (int i = activeSkills.Count - 1; i >= 0; --i)
        {
            if (activeSkills[i])
            {
                Destroy(activeSkills[i].gameObject);
                activeSkills.RemoveAt(i);
            }
        }
    }
    
    protected void InstantiateSkill(BaseSkill skill)
    {
        BaseSkill newSkill = Instantiate(skill, this.transform);
        newSkill.Initialize(this);
        activeSkills.Add(newSkill);
    }
    #endregion 기본 스탯 초기화

    #region 죽음 처리
    /// <summary>
    /// Character가 죽었는지 확인
    /// </summary>
    public bool CheckDeadAndPlayAnim()
    {
        if(health.CheckHealthZero())
        {
            HandleDeath();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Animator를 통해 캐릭터를 죽은 상태로 변경
    /// </summary>
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
            buff.RemoveBuff();
        }
    }
    #endregion

    #region Getter Setter
    public Sprite Portrait => characterStat.portrait;
    public string Name => characterStat.characterName;
    public int Size => characterStat.size;
    public int Cost => characterStat.cost;
    public Health Health => health;
    public SpriteRenderer Sprite => sprite;
    public Stat Stat => stat;

    public bool IsDead => isDead;
    public bool IsAlly
    {
        get { return isAlly; }
        set { isAlly = value; }
    }
    public bool IsMainCharacter => isMainCharacter;

    public bool IsTurnUsed
    {
        get { return isTurnUsed; }
        set { isTurnUsed = value; }
    }

    public bool IsIdle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }

    public int RowOrder
    {
        get { return rowOrder; }
        set
        {
            rowOrder = value;
            anim.SetSortLayer(rowOrder);
        }
    }
    #region 바뀐 스탯 
    public float ChangedSpeed => stat.speed - characterStat.BaseStat.speed;
    public float ChangedDefense => stat.defense - characterStat.BaseStat.defense;
    public float ChangedCrit => stat.crit - characterStat.BaseStat.crit;
    public float ChangedAccuracy => stat.accuracy - characterStat.BaseStat.accuracy;
    public float ChangedEvasion => stat.evasion - characterStat.BaseStat.evasion;
    public float ChangedResist => stat.resist - characterStat.BaseStat.resist;
    public float ChangedMinStat => stat.minStat - characterStat.BaseStat.minStat;
    public float ChangedMaxStat => stat.maxStat - characterStat.BaseStat.maxStat;
    #endregion

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
