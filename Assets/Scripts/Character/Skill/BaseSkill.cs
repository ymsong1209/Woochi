using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BaseSkill
{

    [SerializeField] protected  SkillSO skillSO;
    [Tooltip("SkillOwner를 세팅해줘야함")]
    [SerializeField] private    BaseCharacter skillOwner;

    [SerializeField] private string skillName;
    [SerializeField] private SkillRadius skillRadius;
    [SerializeField] private SkillType skillType;

    /// <summary>
    /// 스킬 적중시 적용시킬 버프 리스트
    /// </summary>
    public List<GameObject> bufflist;

    #region Header SKILL STATS
    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private int minStat;       // 최소 계수
    [SerializeField] private int maxStat;       // 최대 계수
    [SerializeField] private int multiplier;    // 피해량 계수
    [SerializeField] private int skillAccuracy;     // 스킬 명중 수치

    public void Initialize()
    {
        skillName = skillSO.name;
        skillRadius = skillSO.SkillRadius;
        skillType = skillSO.SkillType;
        minStat = skillSO.BaseMinStat;
        maxStat = skillSO.BaseMaxStat;
        multiplier = skillSO.BaseMultiplier;
        skillAccuracy = skillSO.BaseSkillAccuracy;
    }

    public virtual void ApplySkill(BaseCharacter _Opponent)
    {
        //아군 보호 스킬등으로 보호 할 수 있음
        //최종적으로 공격해야하는 적 판정
        BaseCharacter opponet = CheckOpponentValid(_Opponent);

        if(opponet == null)
        {
            Debug.LogError("opponent is null");
            return;
        }

        //명중 체크
        if (CheckAccuracy() == false) return;
        //회피 체크
        if (CheckEvasion(opponet) == false) return;
        else
        {
            //대미지 로직 적용
            
        }
        //버프 체크
        if (CheckApplyBuff(opponet))
        {
            //버프 적용
            foreach (GameObject ApplybuffGameobject in skillSO.bufflist)
            {
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                ApplyBuff(opponet, BufftoApply);
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
    /// 저항 판정
    /// 적이 저항에 성공했으면 false반환
    /// </summary>
    private bool CheckApplyBuff(BaseCharacter _opponet)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponet.Resist) return true;
        return false;
    }

    public virtual void ApplyBuff(BaseCharacter _Opponent, BaseBuff _buff)
    {
        //같은 버프를 넣으려는 경우 중첩 횟수를 더함
        foreach(BaseBuff activebuff in _Opponent.activeBuffs)
        {
            if(activebuff.BuffType == _buff.BuffType)
            {
                activebuff.StackBuff();
                return;
            }
        }
        _Opponent.activeBuffs.Add(_buff);
    }

    public virtual void ApplyStat(BaseCharacter _Opponent, int _minDamage, int _maxDamage, int _multiplier, SkillType _type)
    {
        switch(_type)
        {
            case SkillType.Attack:
            {
                Health opponentHealth = _Opponent.gameObject.GetComponent<Health>();
               
            }
            break;
            case SkillType.Heal:
            {
            
            }
            break;
        }
    }


    #region Getter Setter
    public string Name => skillSO.name;

    public int MinStat => minStat;
    public int MaxStat => maxStat;
    public int Multiplier => multiplier;

    #endregion Getter Setter

}
