using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillScrollSelectedIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private int idx;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Sprite noIconImg;
    public SkillScroll Parent;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hovered");
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.GetInstance.Library.EquipSkill(0,idx);
            Parent.Activate();
            
        }
    }
    
    public void SetIcon(int skillID)
    {
        
        if (skillID == 0)
        {
            icon.sprite = defaultIcon;
        }
        else
        {
            Sprite woochiskillicon = GameManager.GetInstance.Library.GetSkill(skillID).SkillSO.skillIcon;
            if (woochiskillicon)
            {
                icon.sprite = woochiskillicon;
            }
            else
            {
                icon.sprite = noIconImg;
            }
                
        }
    }
    
    public Image Icon
    {
        get => icon;
        set => icon = value;
    }
    public int Idx
    {
        get => idx;
        set => idx = value;
    }
    
}

