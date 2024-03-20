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
        //스킬 테두리 icon이 빛나야함

        //skillSO

    }

    #region Validation
    private void OnValidate()
    {
        
    }
    #endregion
}
