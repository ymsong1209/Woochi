using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private BaseSkill   skill = new BaseSkill();

    public void SetSkill(BaseSkill _skill)
    {
        btn.interactable = true;
        skill = _skill;

        // TODO : 스킬 아이콘 스프라이트 지정
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Skill Name : " + skill.Name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    #region Getter, Setter
    public BaseSkill Skill => skill;
    #endregion
}
