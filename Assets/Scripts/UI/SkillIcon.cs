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
    public  Image       image;
    public  Button      btn;
    public  Transform   tooltipPos;     // ���� ��ġ�� �����ϱ� ����

    private BaseSkill skill;

    public void SetSkill(BaseSkill _skill, bool isEnable = true)
    {
        if (_skill != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = _skill.SkillSO.skillIcon;
            btn.interactable = isEnable;
            skill = _skill;
        }
        //_skill�� null�� ��� �� skill�� �ʱ�ȭ
        else
        {
            image.gameObject.SetActive(false);
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
