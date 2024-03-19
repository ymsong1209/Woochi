using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill
{

    [SerializeField] private SkillSO skillSO;




    private bool isSelected = false;


    public void SetSelect(bool _selected)
    {
        
        if (_selected)
        {
            //기존에 선택된 스킬이 있을 경우
            if (isSelected)
            {

            }
            else
            {

            }
        }
        //스킬 선택 해제
        else
        {

        }

        isSelected = _selected;

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
