using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public class BaseSkill : MonoBehaviour
{
    [SerializeField] private SkillSO skillSO;

    [Tooltip("SkillOwner를 세팅해줘야함")]
    [SerializeField] private    BaseCharacter skillOwner;

    [SerializeField] private string skillName;
    /// <summary>
    /// 스킬을 사용할 수 있는 열
    /// 0~4 : 아군 1~4열
    /// 5~8 : 적군 1~4열
    /// </summary>
    [SerializeField] private bool[] skillAvailableRadius = new bool[8];

    /// <summary>
    /// 스킬을 적용시킬 수 있는 열
    /// 0~4 : 아군 1~4열
    /// 5~8 : 적군 1~4열
    /// </summary>
    [SerializeField] private bool[] skillRadius = new bool[8];

    [SerializeField] private SkillTargetType skillTargetType;
    [SerializeField] private SkillType skillType;

    /// <summary>
    /// 스킬 적중시 적용시킬 버프 리스트
    /// </summary>
    public List<GameObject> bufflist = new List<GameObject>();

    #region Header SKILL STATS
    [Space(10)]
    [Tooltip("Skill Stat은 SkillSO에서 처리함.")]
    [Header("Skill Basics")]
   
    #endregion Header SKILL STATS

    [SerializeField,ReadOnly] private float multiplier;    // 피해량 계수
    [SerializeField,ReadOnly] private float skillAccuracy; // 스킬 명중 수치

    /// <summary>
    /// 자신이 가지고 있는 SkillSO 정보를 이용해 BaseSkill을 초기화
    /// </summary>
    public void Initialize()
    {
        skillName = skillSO.SkillName;
        skillAvailableRadius = skillSO.SkillAvailableRadius;
        skillRadius = skillSO.SkillRadius;
        skillType = skillSO.SkillType;
        multiplier = skillSO.BaseMultiplier;
        skillAccuracy = skillSO.BaseSkillAccuracy;
        skillTargetType = skillSO.SkillTargetType;
        bufflist = new List<GameObject>(skillSO.bufflist);
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
        //아군 보호 스킬등으로 보호 할 수 있음
        //최종적으로 공격해야하는 적 판정
        BaseCharacter opponent = CheckOpponentValid(_Opponent);

        if(opponent == null)
        {
            Debug.LogError("opponent is null");
            return;
        }

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
    }

    protected virtual void ApplySkill(BaseCharacter _opponent)
    {
        bool isCrit = false;
        //공격 실패시 버프 적용 안함
        if (AttackLogic(_opponent, ref isCrit) == false) return;
        
        //치명타일 경우 버프 바로 적용
        if (isCrit)
        {
            foreach (GameObject ApplybuffGameobject in bufflist)
            {
                if (!ApplybuffGameobject) continue;
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                if (!BufftoApply) continue;
                //먼저 buff/debuff가 몇%의 확률로 걸리는지 판단.
                if (CheckApplyBuff(BufftoApply) == false) continue;
                //치명타면 저항 무시한채 스킬 적용
                ApplyBuff(_opponent, BufftoApply);
            }
        }
        else
        {
            foreach (GameObject applybuffGameobject in bufflist)
            {
                if (!applybuffGameobject) continue;
                BaseBuff bufftoApply = applybuffGameobject.GetComponent<BaseBuff>();
                //먼저 buff/debuff가 몇%의 확률로 걸리는지 판단.
                if (CheckApplyBuff(bufftoApply) == false) continue;
                //적의 저항 수치 판단.
                if (CheckResist(_opponent))
                {
                    ApplyBuff(_opponent, bufftoApply);
                }
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
        
        ////
        //TODO : receivers가 한번에 공격받는 듯한 카메라 무빙로직 추가
        ////        

        foreach (BaseCharacter opponent in receivers)
        {
            ApplySkill(opponent);
        };
    }
    bool AttackLogic(BaseCharacter _opponent, ref bool _iscrit)
    {
        //치명타일 경우 명중, 회피, 저항 무시하고 바로 스킬 적용
        if (CheckCrit())
        {
            Debug.Log(skillOwner.ToString() + "uses Skill on "+ skillName + "to "+ _opponent.name.ToString());
            _iscrit = true;
            ApplyStat(_opponent, true);
        }
        
        //명중 체크
        if (CheckAccuracy() == false)
        {
            Debug.Log("Accuracy Failed on" + _opponent.name.ToString());
            skillOwner.ShowDamageUI(AttackResult.Miss);
            return false;
        }
        //회피 체크
        if (CheckEvasion(_opponent) == false)
        {
            Debug.Log(_opponent.name.ToString() + "Evaded skill" + skillName);
            _opponent.ShowDamageUI(AttackResult.Evasion);
            return false;
        }
        
        Debug.Log( skillOwner.ToString() + "uses Non Crit Skill on " + skillName + "to " + _opponent.name.ToString());
        ApplyStat(_opponent, false);

        return true;
    }

    protected BaseCharacter CheckOpponentValid(BaseCharacter _Opponent)
    {
        BaseCharacter finaltarget = _Opponent;
        foreach(BaseBuff buff in finaltarget.activeBuffs)
        {
            if(buff.BuffType == BuffType.Shield)
            {
                ProtectBuff protectbuff = buff as ProtectBuff;
                finaltarget = protectbuff.ProtectionOwner;
            }
        }

        return _Opponent;
    }


    /// <summary>
    /// 명중 판정
    /// 스킬 명중 수치 + 캐릭터 명중 수치로 계산
    /// 명중했을 경우 true 반환
    /// </summary>
    protected bool CheckAccuracy()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillAccuracy + skillOwner.Accuracy) return true;
        else return false;
    }

    /// <summary>
    /// 회피 판정
    /// 적이 회피했으면 false반환
    /// </summary>
    protected bool CheckEvasion(BaseCharacter _opponent)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Evasion) return true;
        return false;
    }

    /// <summary>
    /// 버프를 적용시킬 확률 계산
    /// 버프를 적용시킬 수 있으면 true 반환
    /// </summary>
    protected bool CheckApplyBuff(BaseBuff _buff)
    {
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
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Resist) return true;
        return false;
    }

    /// <summary>
    /// 치명타 판정이 성공하면 true 반환
    /// </summary>
    protected bool CheckCrit()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillOwner.Crit) return true;
        return false;
    }

    public virtual BaseBuff ApplyBuff(BaseCharacter _Opponent, BaseBuff _buff)
    {
        
        BaseBuff activeBuff = FindMatchingBuff(_Opponent, _buff);

        if (activeBuff)
        {
            // 기존 버프와 중첩
            activeBuff.StackBuff();
            return activeBuff;
        }

        // 새 버프 추가
        BaseBuff instantiatedBuff = Instantiate(_buff);
        instantiatedBuff.AddBuff(_Opponent);
        return instantiatedBuff;
    }
    
    private BaseBuff FindMatchingBuff(BaseCharacter _Opponent, BaseBuff _buff)
    {
        foreach (BaseBuff activeBuff in _Opponent.activeBuffs)
        {
            if (activeBuff == null) continue;

            if (activeBuff.BuffType == _buff.BuffType)
            {
                // 스탯 변경 버프는 스탯 변경 버프끼리
                if (_buff.BuffType == BuffType.StatChange)
                {
                    StatBuff activeStatBuff = activeBuff as StatBuff;
                    StatBuff statBuff = _buff as StatBuff;

                    if (activeStatBuff != null && statBuff != null && activeStatBuff.StatBuffName == statBuff.StatBuffName)
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

    private void ApplyStat(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.gameObject.GetComponent<Health>();
        //최소, 최대 대미지 사이의 수치를 고름
        
        float RandomStat = Random.Range(skillOwner.MinStat, skillOwner.MaxStat);
        //피해량 계수를 곱함
        RandomStat *= (multiplier / 100);
       
        switch (skillType)
        {
            case SkillType.Attack:
            {
                //방어 스탯을 뺌
                RandomStat = RandomStat * (100 - _opponent.Defense) / 100;
                if (_isCrit) RandomStat = RandomStat * 2;

                opponentHealth.ApplyDamage((int)Mathf.Round(RandomStat), _isCrit);
                _opponent.CheckDead();
            }
            break;
            case SkillType.Heal:
            {
                if (_isCrit) RandomStat = RandomStat * 2;
                opponentHealth.Heal((int)Mathf.Round(RandomStat));
            }
            break;
        }
    }

    public bool IsSkillAvailable(int _index)
    {
        return skillAvailableRadius[_index];
    }
    #region Getter Setter
    public string Name => skillName;
    public float Multiplier => multiplier;

    public SkillSO SkillSO => skillSO;
    public bool[] SkillAvailableRadius => skillAvailableRadius;
    public bool[] SkillRadius => skillRadius;
    public SkillTargetType SkillTargetType => skillTargetType;
    public BaseCharacter SkillOwner
    {
        get { return skillOwner; }
        set { skillOwner = value; }
    }

    #endregion Getter Setter

}
