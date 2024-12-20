using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BaseCharacterHUD))]
[RequireComponent(typeof(BaseCharacterCollider))]
[DisallowMultipleComponent]
public class BaseCharacter : MonoBehaviour
{
    [HideInInspector] public BaseCharacterHUD HUD;
    [HideInInspector] public BaseCharacterAnimation anim;
    [HideInInspector] public BaseCharacterCollider collider;

    [SerializeField] protected CharacterStatSO characterStat;
    private BuffList buffList;

    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS

    #region Character Stats

    [SerializeField]
    private Health health = new Health();

    public Stat baseStat;
    public Stat levelUpStat;
    public Stat rewardStat;
    public Stat buffStat;
    public Level level;

    #endregion

    [SerializeField] bool isMainCharacter = false;

    #region Header BATTLE STATS

    [Space(10)]
    [Header("Battle Stats")]

    #endregion Header BATTLE STATS

    #region BATTLE STATS

    [SerializeField, ReadOnly]
    protected bool isDead;

    /// <summary>
    /// 나에게 적용된 버프
    /// </summary>
    public List<BaseBuff> activeBuffs = new List<BaseBuff>();

    public List<BaseSkill> activeSkills = new List<BaseSkill>();

    /// <summary>
    /// chatacterStat에 있는 skillSO중 어떤걸 BaseSkill로 넣을 건지 정하는 bool
    /// 크기는 characterStat 내부의 skills의 길이랑 동일해야함.
    /// </summary>
    [SerializeField] protected List<bool> activeSkillCheckBox = new List<bool>();

    [SerializeField, ReadOnly] protected bool isAlly;
    protected bool isTurnUsed = false; //한 라운드 내에서 자신의 턴을 사용했을 경우
    [HideInInspector] public bool canUseTurn = true;
    protected bool isIdle = true;

    public bool isDummy = false; // 우치 소환전용 더미 캐릭터인지
    [HideInInspector] public bool isSummoned = false; // 캐릭터가 소환되었는지
    // 캐릭터가 앞 열에서부터 몇 번째 순서인지
    [SerializeField, ReadOnly] private int rowOrder;

    private bool isSelected = false; // 배틀 중 스킬 대상으로 선택된 캐릭터인지

    #endregion BATTLE STATS

    #region Event

    public Action onHealthChanged;
    public Action<AnimationType> onPlayAnimation;
    public Action<AttackResult, int, bool> onAttacked;
    public Action onLevelUp;

    private void SetCanTurn(int _ID, bool _canUseTurn)
    {
        if (ID == _ID)
        {
            canUseTurn = _canUseTurn;
        }
    }
    #endregion

    private void Awake()
    {
        HUD = GetComponent<BaseCharacterHUD>();
        anim = GetComponent<BaseCharacterAnimation>();
        collider = GetComponent<BaseCharacterCollider>();
        buffList = GetComponentInChildren<BuffList>();

        onLevelUp += LevelUp;
        EventManager.GetInstance.onCanUseTurn += SetCanTurn;
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
        anim.OnSelected(isSelected);
        BattleManager.GetInstance.CharacterSelected(this);
    }

    public void InitSelect()
    {
        isSelected = false;
        anim.OnSelected(isSelected);
    }

    /// <summary>
    /// 전투가 끝날 때 캐릭터의 스탯, 체력을 저장
    /// </summary>
    public virtual void SaveStat()
    {
        CharacterInfoData info = new CharacterInfoData(ID, baseStat, levelUpStat, rewardStat, level, health);
        DataCloud.playerData.SaveInfo(info);
    }

    public void OnMove()
    {
        if (isDead)
        {
            health.TurnToResurrect = Mathf.Clamp(health.TurnToResurrect - 1, 0, DataCloud.countForRessurection);
        }
    }

    #region Skill Check

    public virtual bool CheckUsableSkill()
    {
        for (int i = 0; i < activeSkills.Count; i++)
        {
            BaseSkill skill = activeSkills[i];
            if (IsSkillAvailable(skill))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsSkillAvailable(BaseSkill skill)
    {
        return (IsSkillAbleForFormation(skill) && IsSkillReceiverAble(skill));
    }
    
    /// <summary>
    /// 현재 스킬의 owner가 스킬을 시전할 수 있는 열에 있는지 확인
    /// </summary>
    bool IsSkillAbleForFormation(BaseSkill skill)
    {
        int index = BattleManager.GetInstance.GetCharacterIndex(this);
        return skill.IsSkillAvailable(index);;
    }

    /// <summary>
    /// 스킬의 적용 대상이 존재하는지 확인
    /// </summary>
    bool IsSkillReceiverAble(BaseSkill skill)
    {
        for(int i = 0; i < skill.SkillRadius.Length; ++i)
        {
            //SingleWithoutSelf일 경우 자신을 제외한 대상이 존재하는지 확인
            if(skill.SkillTargetType == SkillTargetType.SingularWithoutSelf && 
               i == BattleManager.GetInstance.GetCharacterIndex(this))
                continue;
            
            if (skill.SkillRadius[i] && BattleManager.GetInstance.IsCharacterThere(i))
                return true;
        }

        return false;
    }
    #endregion
    #region 버프 처리
    /// <summary>
    /// 버프 적용 시점에 따라 적절한 버프 처리 함수 호출
    /// 버프 효과 사용 후 캐릭터가 살아있으면 true반환
    /// 죽었을 경우엔 캐릭터가 가지고 있는 모든 버프 제거 후 사망 처리
    /// </summary>
    public bool TriggerBuff(BuffTiming timing, BaseSkill skill = null)
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
            case BuffTiming.PostHit:
                return TriggerBuffs(buff => buff.ApplyPostHitBuff(skill));
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
    public BaseBuff ApplyBuff(BaseCharacter caster, BaseCharacter receiver, BaseBuff buff)
    {
        //자신의 차례일때 자기 자신에게 버프를 적용할 경우 지속시간+1을 줘야함.
        //자기 자신에게 버프를 주고 턴이 지나가기 때문.
        if (caster && receiver && caster == receiver && BattleManager.GetInstance.currentCharacter == caster)
        {
            if(buff.BuffDurationTurns != -1)
            {
                buff.BuffDurationTurns++;
            }
        }
        //같은 종류가 있는 버프가 활성화되어있는지 먼저 확인
        BaseBuff activeBuff = receiver.FindMatchingBuff(buff);

        //같은 종류의 버프가 이미 존재할경우
        if (activeBuff)
        {
            // 기존 버프와 중첩
            activeBuff.StackBuff(buff);
            Destroy(buff.gameObject);
            return activeBuff;
        }

        // 새 버프 추가
        BaseBuff new_buff = buffList.TransferBuffAtIcon(receiver, buff);
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

    protected virtual void HandleDeath()
    {
        RemoveAllBuff();
        anim.PlayDeadAnimation();
        isDead = true;
        Logger.BattleLog($"{Name}이(가) 사망했습니다.", "사망 처리");
    }

    public void RemoveAllBuff(bool battleEnd = false)
    {
        //모든 버프 순회
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            BaseBuff buff = activeBuffs[i];
            activeBuffs.RemoveAt(i);
            buff.RemoveBuff();
        }
    }

    public void RemoveAllBuffWhenBattleEnd()
    {
        //모든 버프 순회
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            BaseBuff buff = activeBuffs[i];
            if (buff.IsRemoveWhenBattleEnd)
            {
                activeBuffs.RemoveAt(i);
                buff.RemoveBuff();
            }
        }
    }

    //stat buff가 적용되거나 사라지면 자신이 가진 모든 버프 순회해서 stat buff에 있는 스탯 적용
    public void CheckForStatChange()
    {
        buffStat = new Stat();
        
        foreach (BaseBuff buff in activeBuffs)
        {
            if (buff.IsSpecialBuff) continue;
            
            if (buff.BuffEffect == BuffEffect.StatStrengthen)
            {
                StatBuff statBuff = buff as StatBuff;
                buffStat += statBuff.changeStat;
            }
            else if (buff.BuffEffect == BuffEffect.StatWeaken)
            {
                StatDeBuff debuff = buff as StatDeBuff;
                buffStat += debuff.changeStat;
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
            if (activeBuff.BuffEffect != _buff.BuffEffect) continue;

            if (_buff.BuffEffect == BuffEffect.StatStrengthen ||
                _buff.BuffEffect == BuffEffect.StatWeaken || 
                _buff.BuffEffect == BuffEffect.DotCureByDamage ||
                _buff.BuffEffect == BuffEffect.ElementalStatStrengthen ||
                _buff.BuffEffect == BuffEffect.ElementalStatWeaken ||
                _buff.BuffEffect == BuffEffect.Special)
            {
                if (activeBuff.BuffName == _buff.BuffName)
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

    #endregion


    #region 기본 스탯 초기화, 레벨업
    public void InitializeStatSO() => characterStat?.Initialize();

    /// <summary>
    /// 기본 스탯 초기화
    /// </summary>
    public virtual void Initialize()
    {
        InitializeStat();
        InitializeHealth();
        InitializeSkill();
    }
    
    protected void InitializeStat()
    {
        InitializeStatSO();
        baseStat = new Stat(characterStat.BaseStat);
        levelUpStat = new Stat(characterStat.LevelUpStat);
        rewardStat = new Stat(characterStat.RewardStat);
        buffStat = new Stat();
        
        level = new Level(characterStat.Level);
        level.owner = this;
    }

    public virtual void InitializeHealth()
    {
        health.Initialize(this, characterStat.BaseHealth);
        onHealthChanged?.Invoke();

        isDead = (health.CurHealth <= 0);
    }

    public virtual void InitializeSkill()
    {
        DestroyActiveSkills();
        //activeSkills의 size만큼 CharacterStat의 skill을 앞에서부터 가져와서 세팅한다.
        int levelcount = 1;
        for(int i = 0; i < activeSkillCheckBox.Count; ++i)
        {
            if (activeSkillCheckBox[i])
            {
                //아군(소환수)만 레벨에 따라 강화 스킬 추가
                if (isAlly && levelcount < level.rank)
                {
                    levelcount++;
                    InstantiateSkill(characterStat.ReinforcedSkills[i]);
                }
                else
                {
                    InstantiateSkill(characterStat.Skills[i]);
                }
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

    protected void LevelUp()
    {
        baseStat += levelUpStat;
        health.LevelUp();
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
        health.TurnToResurrect = DataCloud.countForRessurection;
        isSummoned = false;
        isIdle = true;
        anim.ResetAnim();
        gameObject.SetActive(false);
    }

    public void Resurrect(bool isTool = false)
    {
        if (health.TurnToResurrect == 0 || isTool)
        {
            health.Resurrect();
            isDead = false;
        }
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
    public int ID => characterStat.ID;
    public Sprite Portrait => characterStat.portrait;
    public Sprite[] Icons => characterStat.icons;
    public string Name => characterStat.characterName;
    public int Size => characterStat.size;
    public int Cost => characterStat.cost;
    public Health Health => health;
    public Stat FinalStat
    {
        get
        {
            Stat stat = (baseStat + rewardStat + buffStat);
            stat.Clamp();
            return stat;
        }
    }

    public Stat BaseStat => baseStat;
    public Stat RewardStat => rewardStat;
    public Stat BuffStat => buffStat;

    public BuffList BuffList => buffList;

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

    #endregion
}
