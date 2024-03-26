using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BaseSkill
{

    [SerializeField] protected  SkillSO skillSO;
    [Tooltip("SkillOwner�� �����������")]
    [SerializeField] private    BaseCharacter skillOwner;

    [SerializeField] private string skillName;
    [SerializeField] private SkillRadius skillRadius;
    [SerializeField] private SkillType skillType;

    /// <summary>
    /// ��ų ���߽� �����ų ���� ����Ʈ
    /// </summary>
    public List<GameObject> bufflist;

    #region Header SKILL STATS
    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private float minStat;       // �ּ� ���
    [SerializeField] private float maxStat;       // �ִ� ���
    [SerializeField] private float multiplier;    // ���ط� ���
    [SerializeField] private float skillAccuracy; // ��ų ���� ��ġ

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
        //�Ʊ� ��ȣ ��ų������ ��ȣ �� �� ����
        //���������� �����ؾ��ϴ� �� ����
        BaseCharacter opponent = CheckOpponentValid(_Opponent);

        if(opponent == null)
        {
            Debug.LogError("opponent is null");
            return;
        }

        //���� üũ
        if (CheckAccuracy() == false) return;
        //ȸ�� üũ
        if (CheckEvasion(opponent) == false) return;
        
        //ġ��Ÿ�� ��� �ٷ� ���� ����
        if (CheckCrit())
        {

            ApplyStat(opponent, minStat, maxStat, multiplier, skillType, true);

            //���� ����
            foreach (GameObject ApplybuffGameobject in skillSO.bufflist)
            {
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                ApplyBuff(opponent, BufftoApply);
            }
        }
        //ġ��Ÿ�� �ƴ� ��� ���� ����
        else if(CheckApplyBuff(opponent))
        {
            //���� ���� �����ϸ� ���� ����
            foreach (GameObject ApplybuffGameobject in skillSO.bufflist)
            {
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                ApplyBuff(opponent, BufftoApply);
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
    /// ���� ����
    /// ��ų ���� ��ġ + ĳ���� ���� ��ġ�� ���
    /// �������� ��� true ��ȯ
    /// </summary>
    private bool CheckAccuracy()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillAccuracy + skillOwner.Accuracy) return true;
        else return false;
    }

    /// <summary>
    /// ȸ�� ����
    /// ���� ȸ�������� false��ȯ
    /// </summary>
    private bool CheckEvasion(BaseCharacter _opponent)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Evasion) return true;
        return false;
    }

    /// <summary>
    /// ���� ����
    /// ���� ���׿� ���������� false��ȯ
    /// </summary>
    private bool CheckApplyBuff(BaseCharacter _opponet)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponet.Resist) return true;
        return false;
    }

    /// <summary>
    /// ġ��Ÿ ������ �����ϸ� true ��ȯ
    /// </summary>
    private bool CheckCrit()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillOwner.Crit) return true;
        return false;
    }

    public virtual void ApplyBuff(BaseCharacter _Opponent, BaseBuff _buff)
    {
        //���� ������ �������� ��� ��ø Ƚ���� ����
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

    public virtual void ApplyStat(BaseCharacter _Opponent, float _minStat, float _maxStat, float _multiplier, SkillType _type, bool _isCrit)
    {
        Health opponentHealth = _Opponent.gameObject.GetComponent<Health>();
        //�ּ�, �ִ� ����� ������ ��ġ�� ��
        float RandomStat = Random.Range(_minStat, _maxStat);
        //���ط� ����� ����
        RandomStat *= _multiplier;
       
        switch (_type)
        {
            case SkillType.Attack:
            {
                //��� ������ ��
                RandomStat = RandomStat * (100 - _Opponent.Defense);
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
    public string Name => skillSO.name;

    public float MinStat => minStat;
    public float MaxStat => maxStat;
    public float Multiplier => multiplier;

    public BaseCharacter SkillOwner
    {
        get { return skillOwner; }
        set { skillOwner = value; }
    }

    #endregion Getter Setter

}
