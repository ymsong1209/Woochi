using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill
{

    [SerializeField] protected SkillSO skillSO;

    
    public virtual void SetSelect(bool _selected)
    {

    }
   
    public virtual void ActivateSkill()
    {
        //��ų �׵θ� icon�� ��������

        //skillSO

    }

    #region Validation
    private void OnValidate()
    {
        
    }
    #endregion
}
