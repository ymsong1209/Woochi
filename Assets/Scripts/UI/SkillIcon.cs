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
        btn.interactable = true;
        skill = _skill;

        // TODO : ��ų ������ ��������Ʈ ����
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
