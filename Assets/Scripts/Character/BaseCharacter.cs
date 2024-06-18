using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(BaseCharacterHUD))]
[RequireComponent(typeof(BaseCharacterCollider))]
[DisallowMultipleComponent]
public class BaseCharacter : MonoBehaviour
{
    public BaseCharacterHUD HUD;
    public BaseCharacterAnimation anim;
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

   
    [Tooltip("특정 위치에서 Spawn되게 하고 싶으면 값 입력.")]
    [SerializeField] private bool       isSpawnSpecific = false;
    [SerializeField] private Vector3    spawnLocation;
    [SerializeField] private Quaternion spawnRotation;
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
    protected List<BaseSkill>   totalSkills = new List<BaseSkill>();

    [SerializeField,ReadOnly] protected bool isAlly;
    protected bool isTurnUsed = false; //한 라운드 내에서 자신의 턴을 사용했을 경우
    protected bool isIdle = true;

    public bool isStarting = false;     // 캐릭터가 전투 시작시 소환되었는지
    public bool isSummoned = false;     // 캐릭터가 소환되었는지

    // 캐릭터가 앞 열에서부터 몇 번째 순서인지
    private int rowOrder;
    
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
    }

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
        bool mightDead = false;

        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            //캐릭터의 턴이 스킵되거나, 캐릭터가 죽을 경우 mightDead를 true로 설정
            int result = applyBuffMethod(activeBuffs[i]);
            if (result != 0)
            {
                if (result == -1)
                {
                    mightDead = true;
                }
            }
           

            if (ShouldRemoveBuff(activeBuffs[i]))
            {
                BaseBuff buff = activeBuffs[i];
                RemoveBuffAtIndex(i);
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
    
    /// <summary>
    /// buff gameobject는 instantiated되어서 opponent에 붙어있음.
    /// </summary>
    /// <returns></returns>
    public virtual BaseBuff ApplyBuff(BaseCharacter _Opponent, BaseBuff _buff)
    {

        BaseBuff activeBuff = _Opponent.FindMatchingBuff(_buff);

        if (activeBuff)
        {
            // 기존 버프와 중첩
            activeBuff.StackBuff(_buff);
            return activeBuff;
        }

        // 새 버프 추가
        BaseBuff new_buff = InstantiateBuffAtIcon(_Opponent, _buff);
        new_buff.AddBuff(_Opponent);
        return new_buff;
    }
    BaseBuff InstantiateBuffAtIcon(BaseCharacter opponent, BaseBuff buff)
    {
        // Find the bufflistcanvas GameObject under the opponent
        Transform buffList = opponent.transform.Find("BuffList");
        if (buffList == null)
        {
            Debug.LogError("buffList not found under opponent" + opponent.gameObject.name.ToString());
            return null;
        }
        
        // 모든 자손을 순회하여 알맞은 BuffIcon을 찾음
        Transform targetChild = FindBuffIconTransform(buffList, buff.BuffType);
        if (targetChild == null)
        {
            Debug.LogError("No matching BuffIcon found under BuffListCanvas");
            return null;
        }
    
        BuffIcon buffIcon = targetChild.GetComponent<BuffIcon>();
        if (buffIcon != null && !buffIcon.gameObject.activeSelf)
        {
            buffIcon.gameObject.SetActive(true);
            buffIcon.Activate();
        }
        
        BaseBuff instantiatedBuff = Instantiate(buff, targetChild);
        return instantiatedBuff;
    }
    
    // 재귀적으로 BuffIcon을 찾는 메서드
    Transform FindBuffIconTransform(Transform parent, BuffType buffType)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            BuffIcon buffIcon = child.GetComponent<BuffIcon>();

            if (buffIcon != null && buffIcon.BuffType == buffType)
            {
                return child;
            }

            Transform foundChild = FindBuffIconTransform(child, buffType);
            if (foundChild != null)
            {
                return foundChild;
            }
        }
        return null;
    }

    private bool ShouldRemoveBuff(BaseBuff buff)
    {
        if (buff.BuffDurationTurns < -1)
        {
            Debug.LogError(buff.name + "의 지속시간이 -1보다 작음");
        }
        return buff.BuffDurationTurns == 0;
    }

    private void RemoveBuffAtIndex(int index)
    {
        BaseBuff removebuff = activeBuffs[index];
        if (removebuff)
        {
            activeBuffs.RemoveAt(index);
            removebuff.RemoveBuff();
        }
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
            activeBuffs.RemoveAt(i);
            buff.RemoveBuff();
        }
    }

    //stat buff가 적용되거나 사라지면 자신이 가진 모든 버프 순회해서 stat buff에 있는 스탯 적용
    public void CheckForStatChange()
    {
        //기존 스탯 다시 초기화
        defense = characterStat.BaseDefense;
        crit = characterStat.BaseCrit;
        accuracy = characterStat.BaseAccuracy;
        evasion = characterStat.BaseEvasion;
        resist = characterStat.BaseResist;
        minStat = characterStat.BaseMinStat;
        maxStat = characterStat.BaseMaxStat;
        speed = characterStat.BaseSpeed;
        
        foreach (BaseBuff buff in activeBuffs)
        {
            if (buff.BuffType == BuffType.StatStrengthen || buff.BuffType == BuffType.StatWeaken)
            {
                defense += buff.ChangeDefense;
                crit += buff.ChangeCrit;
                accuracy += buff.ChangeAccuracy;
                evasion += buff.ChangeEvasion;
                resist += buff.ChangeResist;
                minStat += buff.ChangeMinStat;
                maxStat += buff.ChangeMaxStat;
                speed += buff.ChangeSpeed;
            }
        }
        //스탯이 0 이하로 내려간 경우 0으로 조정
        defense = Mathf.Max(defense, 0);
        crit = Mathf.Max(crit, 0);
        accuracy = Mathf.Max(accuracy, 0);
        evasion = Mathf.Max(evasion, 0);
        resist = Mathf.Max(resist, 0);
        minStat = Mathf.Max(minStat, 0);
        maxStat = Mathf.Max(maxStat, 0);
        speed = Mathf.Max(speed, 0);
    }
    
    /// <summary>
    /// activebuffs에서 _buff와 같은 버프를 찾아 반환
    /// </summary>
    public BaseBuff FindMatchingBuff(BaseBuff _buff)
    {
        foreach (BaseBuff activeBuff in activeBuffs)
        {
            if (activeBuff == null) continue;

            if (activeBuff.BuffType == _buff.BuffType)
            {
                // 스탯 변경 버프는 스탯 변경 버프끼리
                if (_buff.BuffType == BuffType.StatStrengthen || _buff.BuffType == BuffType.StatWeaken)
                {
                    StatBuff activeStatBuff = activeBuff as StatBuff;
                    StatBuff statBuff = _buff as StatBuff;

                    StatDeBuff activeStatDebuff = activeBuff as StatDeBuff;
                    StatDeBuff statDebuff = _buff as StatDeBuff;

                    if ((activeStatBuff != null && statBuff != null && activeStatBuff.StatBuffName == statBuff.StatBuffName) ||
                        (activeStatDebuff != null && statDebuff != null && activeStatDebuff.StatBuffName == statDebuff.StatBuffName))
                    {
                        return activeBuff;
                    }
                }
                else
                {
                    return activeBuff;
                }
            }
        }

        return null;
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
    #endregion 기본 스탯 초기화

    #region 죽음 처리
    /// <summary>
    /// Character가 죽었는지 확인
    /// </summary>
    public bool CheckDead()
    {
        if(health.CheckHealthZero())
        {
            onPlayAnimation(AnimationType.Dead);
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
            buff.RemoveBuff();
        }
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
    public float ChangedSpeed => speed - characterStat.BaseSpeed;
    public float ChangedDefense => defense - characterStat.BaseDefense;
    public float ChangedCrit => crit - characterStat.BaseCrit;
    public float ChangedAccuracy => accuracy - characterStat.BaseAccuracy;
    public float ChangedEvasion => evasion - characterStat.BaseEvasion;
    public float ChangedResist => resist - characterStat.BaseResist;
    public float ChangedMinStat => minStat - characterStat.BaseMinStat;
    public float ChangedMaxStat => maxStat - characterStat.BaseMaxStat;
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
