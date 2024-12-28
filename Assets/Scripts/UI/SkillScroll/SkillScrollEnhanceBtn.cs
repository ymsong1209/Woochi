using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillScrollEnhanceBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SkillScrollDescription skillScrollDescription;
    [SerializeField] private int SkillID;
    [SerializeField] private Button enhanceButton;
    [SerializeField] private SkillScrollEnhanceFinalCheck skillScrollEnhanceFinalCheck;
    [SerializeField] private Button enhanceFinalCheckBG;

    [SerializeField] private Image icon;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Sprite hoveredIcon;

    public void SetSkill(int skillid)
    {
        SkillID = skillid;
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        skillScrollDescription.SetEnhancedSkillDescription();
        icon.sprite = hoveredIcon;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        skillScrollDescription.ResetEnhancedSkillDescription();
        icon.sprite = defaultIcon;
    }

    public void Reset()
    {
        icon.sprite = defaultIcon;
    }

    void Start()
    {
        icon.sprite = defaultIcon;
        enhanceButton.onClick.AddListener(() =>
        {
            skillScrollEnhanceFinalCheck.gameObject.SetActive(true);
            skillScrollEnhanceFinalCheck.SetSkill(SkillID);
            enhanceFinalCheckBG.gameObject.SetActive(true);
            
        });
        enhanceFinalCheckBG.onClick.AddListener(() =>
        {
            skillScrollEnhanceFinalCheck.gameObject.SetActive(false);
            enhanceFinalCheckBG.gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Button EnhanceButton
    {
        get => enhanceButton;
    }
}
