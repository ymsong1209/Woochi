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
            //������ ���õ� ��ų�� ���� ���
            if (isSelected)
            {

            }
            else
            {

            }
        }
        //��ų ���� ����
        else
        {

        }

        isSelected = _selected;

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
