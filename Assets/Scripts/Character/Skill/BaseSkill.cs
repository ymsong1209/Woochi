using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Event = AK.Wwise.Event;

/// <summary>
/// 스킬 결과를 저장할 클래스
/// 크리티컬 결과와 일반 결과를 저장
/// </summary>
public class SkillResult
{
    public BaseCharacter Caster; //스킬을 사용한 캐릭터
    public List<BaseCharacter> Opponent = new List<BaseCharacter>(); //스킬을 적용할 대상
    public SkillType type;
    public List<bool> isHit = new List<bool>();
    public List<bool> isCrit = new List<bool>();

    public void Init()
    {
        type = SkillType.Attack;
        Caster = null;
        Opponent.Clear();
        isHit.Clear();
        isCrit.Clear();
    }

    public bool IsAnyHit()
    {
        foreach (bool hit in isHit)
        {
            if (hit) return true;
        }

        return false;
    }
    public bool IsHit(BaseCharacter character)
    {
        return isHit[Opponent.IndexOf(character)];
    }
    public bool IsHit(int index)
    {
        return isHit[index];
    }
    public bool IsCrit(BaseCharacter character)
    {
        return isCrit[Opponent.IndexOf(character)];
    }
    public bool IsCrit(int index)
    {
        return isCrit[index];
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
    private int skillRandomCount = 0;
    protected List<BuffEffect> buffDescriptionList = new List<BuffEffect>();//스킬에 포함된 버프 설명 무엇 넣을것인지.
    
    /// <summary>
    /// 스킬 적중시 적용시킬 버프 리스트
    /// </summary>
    protected List<GameObject> instantiatedBuffList = new List<GameObject>();

    private float multiplier;    // 피해량 계수
    private float skillAccuracy; // 스킬 명중 수치
    [SerializeField] private bool isAlwaysHit = false;     // 회피, 명중 무시하고 무조건 명중
    
    private SkillResult skillResult = new SkillResult();

    /// <summary>
    /// 자신이 가지고 있는 SkillSO 정보를 이용해 BaseSkill을 초기화
    /// </summary>
    public virtual void Initialize(BaseCharacter owner)
    {
        skillOwner = owner;
        skillName = skillSO.SkillName;
        skillAvailableRadius = skillSO.SkillAvailableRadius;
        skillRadius = skillSO.SkillRadius;
        skillType = skillSO.SkillType;
        skillTargetCount = skillSO.SkillTargetCount;
        skillRandomCount = skillSO.SkillRandomCount;
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
    
    public virtual void SetSkillDescription(TextMeshProUGUI text)
    {
        
    }

    public virtual void ActivateSkill(BaseCharacter _Opponent)
    {
        skillResult.Init();
        skillResult.Caster = skillOwner;
        
        Logger.BattleLog($"\"{skillOwner.Name}\"({skillOwner.RowOrder + 1}열)이(가) \"{_Opponent.Name}\"({_Opponent.RowOrder + 1}열)에게 \"{skillName}\"시전", "스킬 시전(클릭)");
        //아군 보호 스킬등으로 보호 할 수 있음
        //최종적으로 공격해야하는 적 판정
        BaseCharacter opponent = CheckOpponentValid(_Opponent);
        
        if(opponent == null)
        {
            Logger.BattleLog($"opponent가 존재하지 않습니다", "CheckOpponentValid");
            skillResult.Opponent.Add(opponent);
            skillResult.isHit.Add(false);
            skillResult.isCrit.Add(false);
            return;
        }

        skillOwner.onPlayAnimation?.Invoke(skillSO.AnimType);

        //단일공격인 경우 _opponent한테만 공격 로직 적용
        if (skillTargetType == SkillTargetType.Singular)
        {
            ApplySkill(opponent);
        }
        //전체 공격인 경우 skillradius내부의 모든 인물에게 skill 적용
        //만일 skillradius 내부의 특정 인물에게만 로직 적용시키고 싶으면 ApplyMultiple재정의하기
        else if (skillTargetType == SkillTargetType.Multiple)
        { 
            ApplyMultiple();
        }
        else if (skillTargetType == SkillTargetType.SingularWithoutSelf)
        {
            ApplySkillSingleWithoutSelf(opponent);
        }
        else if(skillTargetType == SkillTargetType.Random)
        {
            ApplyRandom();
        }
        
        foreach(var obj in instantiatedBuffList)
        {
            Destroy(obj);
        }
        instantiatedBuffList.Clear();

        bool isHit = false;
        bool isCrit = false;
        foreach(bool hit in skillResult.isHit)
        {
            if(hit)
            {
                isHit = true;
            }
        }
        foreach(bool crit in skillResult.isCrit)
        {
            if(crit)
            {
                isCrit = true;
            }
        }
        BattleManager.GetInstance.OnShakeCamera?.Invoke(isHit, isCrit);
    }
    
    protected virtual void ApplySkill(BaseCharacter _opponent)
    {
        Logger.BattleLog($"\"{skillOwner.Name}\"({skillOwner.RowOrder + 1}열)이(가) \"{_opponent.Name}\"({_opponent.RowOrder + 1}열)에게 \"{skillName}\"시전", "스킬 로직 계산 시작");
        skillResult.Opponent.Add(_opponent);
        bool isCrit = false;

        switch (skillType)
        {
            case SkillType.Attack:
            {
                //공격 관련된건 치명타, 명중, 회피, 저항 판정 다함
                _opponent.onPlayAnimation?.Invoke(AnimationType.Damaged);
                if (AttackLogic(_opponent, ref isCrit) == false) return;
            }
                break;
            case SkillType.Heal:
            {
                //힐 관련된건 치명타 판정만 함. 저항, 회피 판정 없이 바로 스탯 적용
                if (CheckCrit())
                {
                    isCrit = true;
                    skillResult.isCrit.Add(true);
                }
                else
                {
                    skillResult.isCrit.Add(false);
                }

                skillResult.isHit.Add(true);
                _opponent.onPlayAnimation?.Invoke(AnimationType.Heal);
                ApplyStat(_opponent, isCrit);
            }
                break;
            case SkillType.SpecialNegative:
            {
                //공격 관련된건 치명타, 명중, 회피, 저항 판정 다함
                _opponent.onPlayAnimation?.Invoke(AnimationType.Damaged);
                if (AttackLogic(_opponent, ref isCrit) == false) return;
            }
                break;
            case SkillType.SpecialPositive:
            {
                _opponent.onPlayAnimation?.Invoke(AnimationType.Heal);
                //공격 실패시 버프 적용 안함
                if (AttackLogic(_opponent, ref isCrit) == false) return;
            }
                break;
            case SkillType.CustomSkill:
            {
                CustomSkillLogic(_opponent);
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
            if (!CheckApplyBuff(buffToApply,_opponent))
            {
                Destroy(clonedbuff);
                continue;
            }

            // 치명타 여부에 따라 저항 무시 또는 저항 수치 판단
            if (isCrit || buffToApply.IsAlwaysApplyBuff || CheckResist(_opponent))
            {
                _opponent.ApplyBuff(skillOwner, _opponent, buffToApply);
            }
            else
            {
                Destroy(clonedbuff);
            }
        }
    }

    protected virtual void CustomSkillLogic(BaseCharacter _opponent)
    {
        
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
                if (!ally || ally.IsDead) continue;
                
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
                if(!enemy || enemy.IsDead) continue;

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

    //SkillRadius에 있는 적 중 자신을 제외한 적에게 스킬 적용
    //자기 자신을 포함하고 싶으면, 재정의해야함.
    protected virtual void ApplySkillSingleWithoutSelf(BaseCharacter _opponent)
    {
        ApplySkill(_opponent);
    }

    //SkillRadius에 있는 적 중 skillRandomCount만큼 랜덤한 적에게 스킬 적용
    protected virtual void ApplyRandom()
    {
        Formation allies = BattleManager.GetInstance.Allies;
        Formation enemies = BattleManager.GetInstance.Enemies;
        
        List<BaseCharacter> receivers = new List<BaseCharacter>();
        for (int i = 0; i < skillRadius.Length; ++i)
        {
            if (i < 4 && skillRadius[i])
            {
                BaseCharacter ally = allies.formation[i];
                if (!ally || ally.IsDead) continue;

                if (ally.Size == 2)
                {
                    if (!receivers.Any(e => e.gameObject == ally.gameObject))
                    {
                        receivers.Add(ally);
                    }
                }
                else
                {
                    receivers.Add(ally);
                }
            }
            else if (i is >= 4 and < 8 && skillRadius[i])
            {
                BaseCharacter enemy = enemies.formation[i - 4];
                if (!enemy || enemy.IsDead) continue;

                if (enemy.Size == 2)
                {
                    if (!receivers.Any(e => e.gameObject == enemy.gameObject))
                    {
                        receivers.Add(enemy);
                    }
                }
                else
                {
                    receivers.Add(enemy);
                }
            }
        }
        // 수집된 대상에서 skillRandomCount만큼 랜덤한 대상을 선택
        int randomCount = Mathf.Min(skillRandomCount, receivers.Count);
    
        List<BaseCharacter> randomTargets = new List<BaseCharacter>();
        while (randomTargets.Count < randomCount)
        {
            int randomIndex = UnityEngine.Random.Range(0, receivers.Count);
            BaseCharacter selected = receivers[randomIndex];

            // 이미 선택된 대상은 제외하고, 새로운 대상을 추가
            if (!randomTargets.Contains(selected))
            {
                randomTargets.Add(selected);
            }
        }
    
        // 선택된 대상들에게 스킬 적용
        foreach (BaseCharacter target in randomTargets)
        {
            ApplySkill(target);
        }
    }
    bool AttackLogic(BaseCharacter _opponent, ref bool _iscrit)
    {
        // 시나리오 모드에서는 무조건 비크리로 맞춤
        if (DataCloud.IsScenarioMode)
        {
            skillResult.isHit.Add(true);
            skillResult.isCrit.Add(false);
            ApplyStat(_opponent, false);
            return true;
        }
        
        //치명타일 경우 명중, 회피, 저항 무시하고 바로 스킬 적용
        if (CheckCrit())
        {
            _iscrit = true;
            skillResult.isHit.Add(true);
            skillResult.isCrit.Add(true);
            ApplyStat(_opponent, true);
            _opponent.PlayHitSound();
            return true;
        }
        
        //명중 체크
        if (CheckAccuracy(_opponent) == false)
        {
            skillResult.isHit.Add(false);
            skillResult.isCrit.Add(false);
            _opponent.onAttacked(AttackResult.Miss, 0, false);
            return false;
        }
        //회피 체크
        if (CheckEvasion(_opponent) == false)
        {
            skillResult.isHit.Add(false);
            skillResult.isCrit.Add(false);
            _opponent.onAttacked(AttackResult.Evasion, 0, false);
            return false;
        }
        
        skillResult.isHit.Add(true);
        skillResult.isCrit.Add(false);
        ApplyStat(_opponent, false);
        _opponent.PlayHitSound();
        
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
        if (_Opponent != finaltarget)
        {
            Logger.BattleLog($"{skillName}의 최종 목적지 : {_Opponent.Name}", "CheckOpponentValid");
        }
        return finaltarget;
    }


    /// <summary>
    /// 명중 판정
    /// 스킬 명중 수치 + 캐릭터 명중 수치로 계산
    /// 명중했을 경우 true 반환
    /// </summary>
    protected bool CheckAccuracy(BaseCharacter _opponent)
    {
        if (isAlwaysHit) return true;
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillAccuracy + skillOwner.FinalStat.GetValue(StatType.Accuracy)) return true;
        else
        {
            Logger.BattleLog(
                $"\"{skillOwner.Name}\"({skillOwner.RowOrder + 1}열)이 \"{skillName}\" 명중 실패 on \"{_opponent.Name}\"({_opponent.RowOrder + 1}열)\n"+
                $"명중 수치 : {skillAccuracy + skillOwner.FinalStat.GetValue(StatType.Accuracy)}, RandomValue : {RandomValue}", 
                "명중 판정"
            );
            return false;
        }
    }

    /// <summary>
    /// 회피 판정
    /// 적이 회피했으면 false반환
    /// </summary>
    protected bool CheckEvasion(BaseCharacter _opponent)
    {
        if (isAlwaysHit) return true;
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.FinalStat.GetValue(StatType.Evasion)) return true;
        Logger.BattleLog(
            $"\"{_opponent.Name}\"({_opponent.RowOrder + 1}열)이 \"{skillName}\"에 회피\n" +
            $"회피 수치 : {_opponent.FinalStat.GetValue(StatType.Evasion)}, RandomValue : {RandomValue}",
            "회피 판정"
        );
        return false;
    }

    /// <summary>
    /// 버프를 적용시킬 확률 계산
    /// 버프를 적용시킬 수 있으면 true 반환
    /// </summary>
    protected bool CheckApplyBuff(BaseBuff _buff, BaseCharacter _opponent)
    {
        if (_buff.IsAlwaysApplyBuff) return true;

        if (_opponent.IsDead)
        {
            Logger.BattleLog($"\"{skillName}\"내부의 \"{_buff.BuffName}\" 버프 적용 실패 on \"{_opponent.Name}\"({_opponent.RowOrder + 1}열) because is Dead", "버프 적용 가능 여부");
            return false;
        }
        //적용된 버프를 순회하면서 버프를 적용시킬 수 있는지 확인
        foreach(BaseBuff buff in _opponent.activeBuffs)
        {
            if (buff.CanApplyBuff(_buff) == false)
            {
                Logger.BattleLog($"\"{skillName}\"내부의 \"{_buff.name}\" 버프 적용 실패 on \"{_opponent.Name}\"({_opponent.RowOrder + 1}열) because of Buff \"{buff.name}\"", "버프 적용 가능 여부");
                return false;
            }
        }
        
        int RandomValue = Random.Range(0, 100);
        if (RandomValue <= _buff.ChanceToApplyBuff) return true;
        Logger.BattleLog($"\"{skillName}\"내부의 \"{_buff.BuffName}\" 버프 확률 굴림 실패 on \"{_opponent.Name}\"({_opponent.RowOrder + 1}열) with RandomValue {RandomValue}, 버프 걸릴 확률 : {_buff.ChanceToApplyBuff}", "버프 확률 굴림 실패");
        //Debug.Log("\""+skillName+"\""+"내부의 "+"\""+_buff.name+"\"" + "버프 확률 굴림 실패 with RandomValue" + RandomValue + ", 버프 걸릴 확률 : " + _buff.ChanceToApplyBuff);
        return false;
    }

    /// <summary>
    /// 저항 판정
    /// 적이 저항에 성공했으면 false반환
    /// </summary>
    protected bool CheckResist(BaseCharacter _opponent)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.FinalStat.GetValue(StatType.Resist)) return true;
        Logger.BattleLog($"\"{_opponent.Name}\"({_opponent.RowOrder + 1}열)이 \"{skillName}\"에 저항\n"+
                         $"저항 수치 : {_opponent.FinalStat.GetValue(StatType.Resist)}, RandomValue : {RandomValue}", "저항 판정");
        //Debug.Log(_opponent.name + "Resisted skill " + skillName + "with resist" + _opponent.FinalStat.GetValue(StatType.Resist) + ", RandomValue" + RandomValue);
        return false;
    }

    /// <summary>
    /// 치명타 판정이 성공하면 true 반환
    /// </summary>
    protected bool CheckCrit()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillOwner.FinalStat.GetValue(StatType.Crit))
        {
            Logger.BattleLog($"\"{skillOwner.Name}\"({skillOwner.RowOrder + 1}열)이 \"{skillName}\"에 치명타 성공\n"+
                             $"치명타 수치 : {skillOwner.FinalStat.GetValue(StatType.Crit)}, RandomValue : {RandomValue}", "치명타 판정 성공");
            return true;
        }
        return false;
    }
    

    protected virtual int ApplyStat(BaseCharacter receiver, bool isCrit)
    {
        Health opponentHealth = receiver.Health;
       
        switch (skillType)
        {
            case SkillType.Attack:
            {
                float Damage = CalculateDamage(receiver, isCrit);
                //원소 버프.디버프는 대미지 계산 후 적용
                Damage = Mathf.Clamp(CalculateElementalDamageBuff(Damage),0,9999);
                Damage = Mathf.Clamp(CalculateFinalDamage(Damage),0,9999);
                opponentHealth.ApplyDamage((int)Mathf.Round(Damage), isCrit);
                receiver.CheckDeadAndPlayAnim();
                return (int)Mathf.Round(Damage);
            }
            case SkillType.Heal:
            {
                float HealAmount = CalculateHeal(receiver, isCrit);
                opponentHealth.Heal((int)Mathf.Round(HealAmount));
                return (int)Mathf.Round(HealAmount);
            }
            default:
            {
                //특수 스킬은 HP에 영향을 안 미침.
                return 0;
            }
        }
    }
    
    protected virtual float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        Stat finalStat = skillOwner.FinalStat;
        float randomStat = Random.Range(finalStat.GetValue(StatType.MinDamage), finalStat.GetValue(StatType.MaxDamage));
        randomStat *= (multiplier / 100);
        float defense = receiver.FinalStat.GetValue(StatType.Defense);
        randomStat = randomStat * (1 - defense / (defense + 100));
        if (isCrit) randomStat = randomStat * 2;
        return randomStat;
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

    protected float CalculateFinalDamage(float damage)
    {
        foreach (BaseBuff buff in SkillOwner.activeBuffs)
        {
            if (buff.BuffEffect == BuffEffect.Fear)
            {
                FearBuff fearBuff = buff as FearBuff;
                if (fearBuff)
                {
                    damage = damage * (1 - fearBuff.DamageReduction / 100);
                }
            }
        }

        return damage;
    }
    
    protected virtual float CalculateHeal(BaseCharacter receiver, bool isCrit)
    {
        Stat finalStat = skillOwner.FinalStat;
        float RandomStat = Random.Range(finalStat.GetValue(StatType.MinDamage), finalStat.GetValue(StatType.MaxDamage));
        RandomStat *= (multiplier / 100);
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
    

    public bool IsSkillAvailable(int _index)
    {
        return skillAvailableRadius[_index];
    }

    public void PlaySound()
    {
        if (skillSO)
        {
            Event skillSound = skillSO.skillSound;
            skillSound?.Post(gameObject);
        }
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
    public int SkillRandomCount => skillRandomCount;
    
    public List<BuffEffect> BuffDescriptionList => buffDescriptionList;

    #endregion Getter Setter

}
