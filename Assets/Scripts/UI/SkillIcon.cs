using System;
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
    public  Transform   tooltipPos;     // ���� ��ġ�� �����ϱ� ����

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
        if (skill.Name == null)
            return;

        UIManager.GetInstance.SetSkillToolTip(skill, tooltipPos.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.skillTooltip.SetActive(false);
    }

    #region Getter, Setter
    public BaseSkill Skill => skill;
    #endregion
}