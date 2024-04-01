using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI���� ��ų �������� ��Ÿ��
/// </summary>
[DisallowMultipleComponent]
public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public  Image       icon;
    public  Button      btn;

    private BaseSkill   skill = new BaseSkill();

    public void SetSkill(BaseSkill _skill)
    {
        if (_skill!=null)
        {
            btn.interactable = true;
            skill = _skill;
        }
        //_skill�� null�� ��� �� skill�� �ʱ�ȭ
        else
        {
            skill = new BaseSkill();
        }
        // TODO : ��ų ������ ��������Ʈ ����
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Skill Name : " + skill.Name + "���� ���콺 �ö�");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    #region Getter, Setter
    public BaseSkill Skill => skill;
    #endregion
}
