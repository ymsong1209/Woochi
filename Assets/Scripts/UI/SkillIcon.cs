using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private BaseSkill skill; // = new BaseSkill();

    public void SetSkill(BaseSkill _skill, bool isEnable = true)
    {
        if (_skill != null)
        {
            btn.interactable = isEnable;
            skill = _skill;
        }
        //_skill�� null�� ��� �� skill�� �ʱ�ȭ
        else
        {
            btn.interactable = false;
            skill = null;
        }
        // TODO : ��ų ������ ��������Ʈ ����
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill == null)
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
