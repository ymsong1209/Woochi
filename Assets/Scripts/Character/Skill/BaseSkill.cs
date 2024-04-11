using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private float multiplier;    // 피해량 계수
    [SerializeField] private float skillAccuracy; // 스킬 명중 수치

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
    /// </summary>
    public virtual void CheckTurnStart()
    {

    }

    public virtual void ApplySkill(BaseCharacter _Opponent)
    {
        //아군 보호 스킬등으로 보호 할 수 있음
        //최종적으로 공격해야하는 적 판정
        BaseCharacter opponent = CheckOpponentValid(_Opponent);

        if(opponent == null)
        {
            Debug.LogError("opponent is null");
            return;
        }

        //명중 체크
        if (CheckAccuracy() == false)
        {
            Debug.Log("Accuracy Failed on" + _Opponent.name.ToString());
            return;
        }
        //회피 체크
        if (CheckEvasion(opponent) == false)
        {
            Debug.Log(_Opponent.name.ToString() + "Evaded skill" + skillName);
            return;
        }
        
        //치명타일 경우 바로 버프 적용
        if (CheckCrit())
        {
            Debug.Log("Crit Skill on "+ skillName + "to "+ _Opponent.name.ToString());
            ApplyStat(opponent, true);

            //버프 적용
            foreach (GameObject ApplybuffGameobject in bufflist)
            {
                if (ApplybuffGameobject == null) continue;
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                if (BufftoApply == null) continue;
                //먼저 buff/debuff가 몇%의 확률로 걸리는지 판단.
                if (CheckApplyBuff(BufftoApply) == false) continue;
                //치명타면 저항 무시한채 스킬 적용
                ApplyBuff(opponent, BufftoApply);
            }
        }
        else
        {
            Debug.Log("Non Crit Skill on " + skillName + "to " + _Opponent.name.ToString());
            ApplyStat(opponent, false);

            foreach (GameObject ApplybuffGameobject in bufflist)
            {
                if (ApplybuffGameobject == null) continue;
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                //먼저 buff/debuff가 몇%의 확률로 걸리는지 판단.
                if (CheckApplyBuff(BufftoApply) == false) continue;
                //적의 저항 수치 판단.
                if (CheckResist(opponent) == false)
                {
                    ApplyBuff(opponent, BufftoApply);
                }
             
            }
        }
        
    }

    BaseCharacter CheckOpponentValid(BaseCharacter _Opponent)
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
    private bool CheckAccuracy()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillAccuracy + skillOwner.Accuracy) return true;
        else return false;
    }

    /// <summary>
    /// 회피 판정
    /// 적이 회피했으면 false반환
    /// </summary>
    private bool CheckEvasion(BaseCharacter _opponent)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Evasion) return true;
        return false;
    }

    /// <summary>
    /// 버프를 적용시킬 확률 계산
    /// 버프를 적용시킬 수 있으면 true 반환
    /// </summary>
    private bool CheckApplyBuff(BaseBuff _buff)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue <= _buff.ChanceToApplyBuff) return true;
        return false;
    }

    /// <summary>
    /// 저항 판정
    /// 적이 저항에 성공했으면 false반환
    /// </summary>
    private bool CheckResist(BaseCharacter _opponent)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Resist) return true;
        return false;
    }

    /// <summary>
    /// 치명타 판정이 성공하면 true 반환
    /// </summary>
    private bool CheckCrit()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillOwner.Crit) return true;
        return false;
    }

    public virtual void ApplyBuff(BaseCharacter _Opponent, BaseBuff _buff)
    {
        //같은 버프를 넣으려는 경우 중첩 횟수를 더함
        foreach(BaseBuff activebuff in _Opponent.activeBuffs)
        {
            if (activebuff == null) continue;
            if(activebuff.BuffType == _buff.BuffType)
            {
                activebuff.StackBuff();
                return;
            }
        }
        _Opponent.activeBuffs.Add(_buff);
    }

    private void ApplyStat(BaseCharacter _Opponent, bool _isCrit)
    {
        Health opponentHealth = _Opponent.gameObject.GetComponent<Health>();
        //최소, 최대 대미지 사이의 수치를 고름
        
        float RandomStat = Random.Range(skillOwner.MinStat, skillOwner.MaxStat);
        //피해량 계수를 곱함
        RandomStat *= (multiplier / 100);
       
        switch (skillType)
        {
            case SkillType.Attack:
            {
                //방어 스탯을 뺌
                RandomStat = RandomStat * (100 - _Opponent.Defense) / 100;
                if (_isCrit) RandomStat = RandomStat * 2;
                opponentHealth.ApplyDamage((int)RandomStat);
            }
            break;
            case SkillType.Heal:
            {
                if (_isCrit) RandomStat = RandomStat * 2;
                opponentHealth.Heal((int)RandomStat);
            }
            break;
        }
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
