using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public class BaseSkill : MonoBehaviour
{
    [SerializeField] private SkillSO skillSO;

    [Tooltip("SkillOwner�� �����������")]
    [SerializeField] private    BaseCharacter skillOwner;

    [SerializeField] private string skillName;
    /// <summary>
    /// ��ų�� ����� �� �ִ� ��
    /// 0~4 : �Ʊ� 1~4��
    /// 5~8 : ���� 1~4��
    /// </summary>
    [SerializeField] private bool[] skillAvailableRadius = new bool[8];

    /// <summary>
    /// ��ų�� �����ų �� �ִ� ��
    /// 0~4 : �Ʊ� 1~4��
    /// 5~8 : ���� 1~4��
    /// </summary>
    [SerializeField] private bool[] skillRadius = new bool[8];

    [SerializeField] private SkillTargetType skillTargetType;
    [SerializeField] private SkillType skillType;

    /// <summary>
    /// ��ų ���߽� �����ų ���� ����Ʈ
    /// </summary>
    public List<GameObject> bufflist = new List<GameObject>();

    #region Header SKILL STATS
    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private float multiplier;    // ���ط� ���
    [SerializeField] private float skillAccuracy; // ��ų ���� ��ġ

    /// <summary>
    /// �ڽ��� ������ �ִ� SkillSO ������ �̿��� BaseSkill�� �ʱ�ȭ
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
    /// �ڽ��� ���ʰ� ���۵ɶ� �������� �ִ��� Ȯ��
    /// </summary>
    public virtual void CheckTurnStart()
    {

    }

    public virtual void ActivateSkill(BaseCharacter _Opponent)
    {
        //�Ʊ� ��ȣ ��ų������ ��ȣ �� �� ����
        //���������� �����ؾ��ϴ� �� ����
        BaseCharacter opponent = CheckOpponentValid(_Opponent);

        if(opponent == null)
        {
            Debug.LogError("opponent is null");
            return;
        }

        //���ϰ����� ��� _opponent���׸� ���� ���� ����
        if (skillTargetType == SkillTargetType.Singular)
        {
           ApplySkill(opponent);
        }
        //��ü ������ ��� skillradius������ ��� �ι����� skill ����
        //���� skillradius ������ Ư�� �ι����Ը� ���� �����Ű�� ������ ApplyMultiple�������ϱ�
        else if (skillTargetType == SkillTargetType.Multiple)
        { 
            ApplyMultiple();
        }
    }

    protected virtual void ApplySkill(BaseCharacter _opponent)
    {
        bool isCrit = false;
        AttackLogic(_opponent, ref isCrit);
        //ġ��Ÿ�� ��� ���� �ٷ� ����
        if (isCrit)
        {
            foreach (GameObject ApplybuffGameobject in bufflist)
            {
                if (!ApplybuffGameobject) continue;
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                if (!BufftoApply) continue;
                //���� buff/debuff�� ��%�� Ȯ���� �ɸ����� �Ǵ�.
                if (CheckApplyBuff(BufftoApply) == false) continue;
                //ġ��Ÿ�� ���� ������ä ��ų ����
                ApplyBuff(_opponent, BufftoApply);
            }
        }
        else
        {
            foreach (GameObject applybuffGameobject in bufflist)
            {
                if (!applybuffGameobject) continue;
                BaseBuff bufftoApply = applybuffGameobject.GetComponent<BaseBuff>();
                //���� buff/debuff�� ��%�� Ȯ���� �ɸ����� �Ǵ�.
                if (CheckApplyBuff(bufftoApply) == false) continue;
                //���� ���� ��ġ �Ǵ�.
                if (CheckResist(_opponent))
                {
                    ApplyBuff(_opponent, bufftoApply);
                }
            }
        }
    }

    //SkillRadius�� �ִ� ���� ��ü���� ��ų ����
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
                
                //�Ʊ��� Size�� 2�� ���
                if (ally.Size == 2)
                {
                    // �̹� Receivers ����Ʈ�� ������ GameObject�� �����ϴ� BaseCharacter�� ���� ��쿡�� �߰�
                    if (!receivers.Any(e => e.gameObject == ally.gameObject))
                    {
                        receivers.Add(ally);
                    }
                }
                else
                {
                    // Size�� 1�� Ally�� �׳� �߰�
                    receivers.Add(ally);
                }
            }
            else if (i is >= 4 and < 8 && skillRadius[i])
            {
                BaseCharacter enemy = enemies.formation[i - 4];
                if(!enemy) continue;

                //���� Size�� 2�� ���
                if(enemy.Size == 2)
                {
                    // �̹� Receivers ����Ʈ�� ������ GameObject�� �����ϴ� BaseCharacter�� ���� ��쿡�� �߰�
                    if (!receivers.Any(e => e.gameObject == enemy.gameObject))
                    {
                        receivers.Add(enemy);
                    }
                }
                else
                {
                    // Size�� 1�� ���� �׳� �߰�
                    receivers.Add(enemy);
                }

            }
        }
        
        ////
        //TODO : receivers�� �ѹ��� ���ݹ޴� ���� ī�޶� �������� �߰�
        ////        

        foreach (BaseCharacter opponent in receivers)
        {
            ApplySkill(opponent);
        };
    }
    bool AttackLogic(BaseCharacter _Opponent, ref bool _iscrit)
    {
        //���� üũ
        if (CheckAccuracy() == false)
        {
            Debug.Log("Accuracy Failed on" + _Opponent.name.ToString());
            return false;
        }
        //ȸ�� üũ
        if (CheckEvasion(_Opponent) == false)
        {
            Debug.Log(_Opponent.name.ToString() + "Evaded skill" + skillName);
            return false;
        }
        
        //ġ��Ÿ�� ��� �ٷ� ���� ����
        if (CheckCrit())
        {
            Debug.Log("Crit Skill on "+ skillName + "to "+ _Opponent.name.ToString());
            _iscrit = true;
            ApplyStat(_Opponent, true);
        }
        else
        {
            Debug.Log("Non Crit Skill on " + skillName + "to " + _Opponent.name.ToString());
            ApplyStat(_Opponent, false);
        }

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
    /// ���� ����
    /// ��ų ���� ��ġ + ĳ���� ���� ��ġ�� ���
    /// �������� ��� true ��ȯ
    /// </summary>
    protected bool CheckAccuracy()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillAccuracy + skillOwner.Accuracy) return true;
        else return false;
    }

    /// <summary>
    /// ȸ�� ����
    /// ���� ȸ�������� false��ȯ
    /// </summary>
    protected bool CheckEvasion(BaseCharacter _opponent)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Evasion) return true;
        return false;
    }

    /// <summary>
    /// ������ �����ų Ȯ�� ���
    /// ������ �����ų �� ������ true ��ȯ
    /// </summary>
    protected bool CheckApplyBuff(BaseBuff _buff)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue <= _buff.ChanceToApplyBuff) return true;
        return false;
    }

    /// <summary>
    /// ���� ����
    /// ���� ���׿� ���������� false��ȯ
    /// </summary>
    protected bool CheckResist(BaseCharacter _opponent)
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue > _opponent.Resist) return true;
        return false;
    }

    /// <summary>
    /// ġ��Ÿ ������ �����ϸ� true ��ȯ
    /// </summary>
    protected bool CheckCrit()
    {
        int RandomValue = Random.Range(0, 100);
        if (RandomValue < skillOwner.Crit) return true;
        return false;
    }

    public virtual void ApplyBuff(BaseCharacter _Opponent, BaseBuff _buff)
    {
        BaseBuff instantiatedBuff = Instantiate(_buff);
        //���� ������ �������� ��� ��ø Ƚ���� ����
        foreach(BaseBuff activebuff in _Opponent.activeBuffs)
        {
            if (activebuff == null) continue;
            if(activebuff.BuffType == instantiatedBuff.BuffType)
            {
                activebuff.StackBuff();
                return;
            }
        }
        instantiatedBuff.AddBuff(_Opponent);
    }

    private void ApplyStat(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.gameObject.GetComponent<Health>();
        //�ּ�, �ִ� ����� ������ ��ġ�� ��
        
        float RandomStat = Random.Range(skillOwner.MinStat, skillOwner.MaxStat);
        //���ط� ����� ����
        RandomStat *= (multiplier / 100);
       
        switch (skillType)
        {
            case SkillType.Attack:
            {
                //��� ������ ��
                RandomStat = RandomStat * (100 - _opponent.Defense) / 100;
                if (_isCrit) RandomStat = RandomStat * 2;

                opponentHealth.ApplyDamage((int)Mathf.Round(RandomStat));
                if (_opponent.CheckDead())
                {
                    _opponent.SetDead();
                };
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
