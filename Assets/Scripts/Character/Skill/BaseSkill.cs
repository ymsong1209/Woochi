using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BaseSkill
{

    [SerializeField] protected SkillSO skillSO;

    
    public virtual void ApplySkill(BaseCharacter _Opponent)
    {



        //버프 적용
        foreach (GameObject ApplybuffGameobject in skillSO.bufflist)
        {
            BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
            ApplyBuff(_Opponent, BufftoApply);
        }

    }


    public virtual bool ApplyBuff(BaseCharacter _Opponent, BaseBuff _buff)
    {
        //버프 적용
        foreach (BaseBuff opponentbuff in _Opponent.activeBuffs)
        {
            if (!opponentbuff.ValidateApplyBuff(_buff.BuffType))
            {
                return false;
            }
        }
        //모든 검사 끝났으면 버프 적용 가능
        return true;
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
    #endregion Getter Setter

}
