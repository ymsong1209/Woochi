using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI에서 스킬 아이콘을 나타냄
/// </summary>
[DisallowMultipleComponent]
public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public  Image       icon;
    public  Button      btn;
    public  Transform   tooltipPos;     // 툴팁 위치를 지정하기 위해

    private BaseSkill skill; // = new BaseSkill();

    public void SetSkill(BaseSkill _skill, bool isEnable = true)
    {
        if (_skill != null)
        {
            btn.interactable = isEnable;
            skill = _skill;
        }
        //_skill이 null일 경우 빈 skill로 초기화
        else
        {
            btn.interactable = false;
            skill = null;
        }
        // TODO : 스킬 아이콘 스프라이트 지정
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
