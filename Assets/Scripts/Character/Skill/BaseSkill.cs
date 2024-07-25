using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 스킬 결과를 저장할 클래스
/// 크리티컬 결과와 일반 결과를 저장
/// </summary>
public class SkillResult
{
    public BaseCharacter Caster; //스킬을 사용한 캐릭터
    public BaseCharacter Opponent; //스킬을 적용할 대상
    public bool isHit = false;
    public bool isCrit = false;

    public void Init()
    {
        isHit = false;
        isCrit = false;
        Caster = null;
        Opponent = null;
    }
}

public class BaseSkill : MonoBehaviour
{
    [SerializeField] private SkillSO skillSO;
    
    private BaseCharacter skillOwner;

    private string skillName;
    private bool[] skillAvailableRadius = new bool[8];
    private bool[] skillRadius = new bool[8];
    private SkillTargetType skillTargetType;
    private SkillType skillType;
    [SerializeField] private int skillTargetCount = 1;
    private List<GameObject> buffPrefabList = new List<GameObject>();
    
    /// <summary>
    /// 스킬 적중시 적용시킬 버프 리스트
    /// </summary>
    protected List<GameObject> instantiatedBuffList = new List<GameObject>();

    private float multiplier;    // 피해량 계수
    private float skillAccuracy; // 스킬 명중 수치
    [SerializeField] private bool isAlwaysHit = false;     // 회피, 명중 무시하고 무조건 명중
    [SerializeField] private bool isAlwaysApplyBuff = false;// 버프를 걸 확률, 저항 판정 무시하고 무조건 적용
    private SkillResult skillResult = new SkillResult();

    /// <summary>
    /// 자신이 가지고 있는 SkillSO 정보를 이용해 BaseSkill을 초기화
    /// </summary>
    public void Initialize(BaseCharacter owner)
    {
        skillOwner = owner;
        skillName = skillSO.SkillName;
        skillAvailableRadius = skillSO.SkillAvailableRadius;
        skillRadius = skillSO.SkillRadius;
        skillType = skillSO.SkillType;
        skillTargetCount = skillSO.SkillTargetCount;
        skillTargetType = skillSO.SkillTargetType;
        multiplier = skillSO.BaseMultiplier;
        skillAccuracy = skillSO.BaseSkillAccuracy;
        buffPrefabList = new List<GameObject>(skillSO.bufflist);
    }

    /// <summary>
    /// 자신의 차례가 시작될때 변경점이 있는지 확인
    /// 원래는 basecharacter의 checkskillsonturnstart에서 호출하지만
    /// 일단 보류
    /// </summary>
    public virtual void CheckTurnStart()
    {

    }

    public virtual void ActivateSkill(BaseCharacter _Opponent)
    {
        skillResult.Init();
        skillResult.Caster = skillOwner;
        //아군 보호 스킬등으로 보호 할 수 있음
        //최종적으로 공격해야하는 적 판정
        skillResult.Opponent = CheckOpponentValid(_Opponent);

        if(skillResult.Opponent == null)
        {
            Debug.LogError("opponent is null");
            return;
        }

        skillOwner.onPlayAnimation?.Invoke(skillSO.AnimType);

        //단일공격인 경우 _opponent한테만 공격 로직 적용
        if (skillTargetType == SkillTargetType.Singular)
        {
           ApplySkill(skillResult.Opponent);
        }
        //전체 공격인 경우 skillradius내부의 모든 인물에게 skill 적용
        //만일 skillradius 내부의 특정 인물에게만 로직 적용시키고 싶으면 ApplyMultiple재정의하기
        else if (skillTargetType == SkillTargetType.Multiple)
        { 
            ApplyMultiple();
        }
        
        foreach(var obj in instantiatedBuffList)
        {
            Destroy(obj);
        }
        instantiatedBuffList.Clear();

        BattleManager.GetInstance.OnShakeCamera?.Invoke(skillResult.isHit, skillResult.isCrit);
    }
    
    protected virtual void ApplySkill(BaseCharacter _opponent)
    {
        bool isCrit = false;

        switch (skillType)
        {
            case SkillType.Heal:
            {
                //힐 관련된건 치명타 판정만 함. 저항, 회피 판정 없이 바로 스탯 적용
                if (CheckCrit())
                {
                    isCrit = true;
                    skillResult.isCrit = true;
                }
                skillResult.isHit = true;
                _opponent.onPlayAnimation?.Invoke(AnimationType.Heal);
                ApplyStat(_opponent, isCrit);
            }
                break;
            default:
            {
                //공격 실패시 버프 적용 안함
                if (AttackLogic(_opponent, ref isCrit) == false) return;
            }
                break;
        }
       

        foreach (GameObject applyBuffGameObject in instantiatedBuffList)
        {
            if (!applyBuffGameObject) continue;
            GameObject clonedbuff = Instantiate(applyBuffGameObject);

            BaseBuff buffToApply = clonedbuff.GetComponent<BaseBuff>();
            if (!buffToApply)
            {
                Destroy(clonedbuff);
                continue;
            }

            // 버프 적용 가능 여부 판단
            if (!CheckApplyBuff(buffToApply))
            {
                Destroy(clonedbuff);
                continue;
            }

            // 치명타 여부에 따라 저항 무시 또는 저항 수치 판단
            if (isCrit || CheckResist(_opponent))
            {
                _opponent.ApplyBuff(skillOwner, _opponent, buffToApply);
            }
            else
            {
                Destroy(clonedbuff);
            }
        }
    }

    //SkillRadius에 있는 적들 전체에게 스킬 적용
    protected virtual void ApplyMultiple()
    {
        Formation allies = BattleManager.GetInstance.Allies;
        Formation enemies = BattleManager.GetInstance.Enemies;
        
        List<BaseCharacter> receivers = new List<BaseCharacter>();
        for (int i = 0; i < skillRadius.Length; ++i)
        {
            if (i<4 && skillRadius[i])
            {
                BaseCharacter ally = allies.formation[i];
                if (!ally) continue;
                
                //아군의 Size가 2인 경우
                if (ally.Size == 2)
                {
                    // 이미 Receivers 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                    if (!receivers.Any(e => e.gameObject == ally.gameObject))
                    {
                        receivers.Add(ally);
                    }
                }
                else
                {
                    // Size가 1인 Ally은 그냥 추가
                    receivers.Add(ally);
                }
            }
            else if (i is >= 4 and < 8 && skillRadius[i])
            {
                BaseCharacter enemy = enemies.formation[i - 4];
                if(!enemy) continue;

                //적의 Size가 2인 경우
                if(enemy.Size == 2)
                {
                    // 이미 Receivers 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                    if (!receivers.Any(e => e.gameObject == enemy.gameObject))
                    {
                        receivers.Add(enemy);
                    }
                }
                else
                {
                    // Size가 1인 적은 그냥 추가
                    receivers.Add(enemy);
                }

            }
        }
        foreach (BaseCharacter opponent in receivers)
        {
            ApplySkill(opponent);
        }
    }
    bool AttackLogic(BaseCharacter _opponent, ref bool _iscrit)
    {
        _opponent.onPlayAnimation?.Invoke(AnimationType.Damaged);

        //치명타일 경우 명중, 회피, 저항 무시하고 바로 스킬 적용
        if (CheckCrit())
        {
            Debug.Log(skillOwner.ToString() + "uses Crit Skill on "+ skillName + "to "+ _opponent.name.ToString());
            _iscrit = true;
            skillResult.isHit = true;
            skillResult.isCrit = true;
            ApplyStat(_opponent, true);
            return true;
        }
        
        //명중 체크
        if (CheckAccuracy() == false)
        {
            Debug.Log("Accuracy Failed on" + _opponent.name.ToString());
            _opponent.onAttacked(AttackResult.Miss, 0, false);
            return false;
        }
        //회피 체크
        if (CheckEvasion(_opponent) == false)
        {
            Debug.Log(_opponent.name.ToString() + "Evaded skill" + skillName);
            _opponent.onAttacked(AttackResult.Evasion, 0, false);
            return false;
        }
        
        skillResult.isHit = true;
        Debug.Log( skillOwner.ToString() + "uses Non Crit Skill on " + skillName + "to " + _opponent.name.ToString());
        ApplyStat(_opponent, false);

        return true;
    }

    protected BaseCharacter CheckOpponentValid(BaseCharacter _Opponent)
    {
        BaseCharacter finaltarget = _Opponent;
        foreach(BaseBuff buff in finaltarget.activeBuffs)
        {
            if(buff.BuffEffect == BuffEffect.Shield)
            {
                ProtectBuff protectbuff = buff as ProtectBuff;
                if(protectbuff) finaltarget = protectbuff.ProtectionOwner;
            }
        }

        return finaltarget;
    }


    /// <summary>
    /// 명중 판정
    /// 스킬 명중 수치 + 캐릭터 명중 수치로 계산
    /// 명중했을 경우 true 반환
    /// </summary>
    protected bool CheckAccuracy()
    {
        if (isAlwaysHit) return true;
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillAccuracy + skillOwner.Stat.accuracy) return true;
        else return false;
    }

    /// <summary>
    /// 회피 판정
    /// 적이 회피했으면 false반환
    /// </summary>
    protected bool CheckEvasion(BaseCharacter _opponent)
    {
        if (isAlwaysHit) return true;
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Stat.evasion) return true;
        return false;
    }

    /// <summary>
    /// 버프를 적용시킬 확률 계산
    /// 버프를 적용시킬 수 있으면 true 반환
    /// </summary>
    protected bool CheckApplyBuff(BaseBuff _buff)
    {
        if (isAlwaysApplyBuff) return true;
        int RandomValue = Random.Range(0, 100);
        if (RandomValue <= _buff.ChanceToApplyBuff) return true;
        return false;
    }

    /// <summary>
    /// 저항 판정
    /// 적이 저항에 성공했으면 false반환
    /// </summary>
    protected bool CheckResist(BaseCharacter _opponent)
    {
        if (isAlwaysApplyBuff) return true;
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Stat.resist) return true;
        return false;
    }

    /// <summary>
    /// 치명타 판정이 성공하면 true 반환
    /// </summary>
    protected bool CheckCrit()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillOwner.Stat.crit) return true;
        return false;
    }
    

    protected virtual void ApplyStat(BaseCharacter receiver, bool isCrit)
    {
        Health opponentHealth = receiver.Health;
       
        switch (skillType)
        {
            case SkillType.Attack:
            {
                float Damage = CalculateDamage(receiver, isCrit);
                Damage = Mathf.Clamp(CalculateElementalDamageBuff(Damage),0,9999);
                opponentHealth.ApplyDamage((int)Mathf.Round(Damage), isCrit);
                receiver.CheckDeadAndPlayAnim();
            }
            break;
            case SkillType.Heal:
            {
                float HealAmount = CalculateHeal(receiver, isCrit);
                opponentHealth.Heal((int)Mathf.Round(HealAmount));
            }
            break;
            case SkillType.Special:
            {
                //특수 스킬은 HP에 영향을 안 미침.
            }
            break;
        }
    }
    
    protected virtual float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        float RandomStat = Random.Range(skillOwner.Stat.minStat, skillOwner.Stat.maxStat);
        RandomStat *= (multiplier / 100);
        RandomStat = RandomStat * (1 - receiver.Stat.defense/(receiver.Stat.defense + 100));
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
    
    protected float CalculateElementalDamageBuff(float damage)
    {
        foreach (BaseBuff buff in SkillOwner.activeBuffs)
        {
            if (buff.BuffEffect == BuffEffect.ElementalStatStrengthen ||
                buff.BuffEffect == BuffEffect.ElementalStatWeaken)
            {
                ElementalStatBuff elementalStatBuff = buff as ElementalStatBuff;
                if (elementalStatBuff && elementalStatBuff.Element == skillSO.SkillElement)
                {
                    damage += elementalStatBuff.ChangeStat;
                }
                ElementalStatDeBuff elementalStatDeBuff = buff as ElementalStatDeBuff;
                if (elementalStatDeBuff && elementalStatDeBuff.Element == skillSO.SkillElement)
                {
                    damage += elementalStatDeBuff.ChangeStat;
                }
            }
        }

        return damage;
    }
    protected virtual float CalculateHeal(BaseCharacter receiver, bool isCrit)
    {
        float RandomStat = Random.Range(skillOwner.Stat.minStat, skillOwner.Stat.maxStat);
        RandomStat *= (multiplier / 100);
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
    

    public bool IsSkillAvailable(int _index)
    {
        return skillAvailableRadius[_index];
    }
    #region Getter Setter
    public string Name => skillName;
    public float Multiplier => multiplier;

    public SkillResult SkillResult => skillResult;
    public SkillSO SkillSO => skillSO;
    
    public List<GameObject> BuffPrefabList => buffPrefabList;
    public bool[] SkillAvailableRadius => skillAvailableRadius;
    public bool[] SkillRadius
    {
        get => skillRadius;
        set => skillRadius = value;
    }

    public SkillTargetType SkillTargetType => skillTargetType;
    public int SkillTargetCount => skillTargetCount;
    public BaseCharacter SkillOwner
    {
        get => skillOwner;
        set => skillOwner = value;
    }

    #endregion Getter Setter

}
