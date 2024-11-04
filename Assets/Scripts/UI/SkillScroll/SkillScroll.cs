using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillScroll : MonoBehaviour
{
    [SerializeField] private Sprite noiconImg;
    
    //우치가 선택한 스킬들
    [SerializeField] private Sprite selectedIconDefault;
    [SerializeField] private Image[] selectedIcons = new Image[5];
    
    //도술 두루마리에 있는 아이콘들
    [SerializeField] private SkillScrollIcon[] skillScrollIcons = new SkillScrollIcon[25];
    [SerializeField] private bool iconSelected;
    
    //도술 설명
    [SerializeField] private SkillScrollDescription skillScrollDescription;

    public Action<int> OnIconHovered;
    public Action<int> OnIconHoverExit;
    public Action<int> OnSkillSelected;
    
    void Start()
    {
        Activate();
        OnIconHovered += Highlight;
        OnIconHoverExit += HighlightExit;
        OnSkillSelected += RemoveSelectedImg;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        iconSelected = false;
        //우치가 선택한 도술 세팅
        for (int i = 0; i < DataCloud.playerData.currentskillIDs.Length; i++)
        {
            int skillID = DataCloud.playerData.currentskillIDs[i];
            if (skillID == 0)
            {
                selectedIcons[i].sprite = selectedIconDefault;
            }
            else
            {
                Sprite woochiskillicon = GameManager.GetInstance.Library.GetSkill(skillID).SkillSO.skillIcon;
                if (woochiskillicon)
                {
                    selectedIcons[i].sprite = woochiskillicon;
                }
                else
                {
                    selectedIcons[i].sprite = noiconImg;
                }
                
            }
        }
        
        //도술 두루마리에 가지고 있는 도술 세팅
        for (int i = 0; i < DataCloud.playerData.totalSkillIDs.GetLength(0); i++)
        {
            for (int j = 0; j < DataCloud.playerData.totalSkillIDs.GetLength(1); j++)
            {
                int skillID = DataCloud.playerData.totalSkillIDs[i, j];
                skillScrollIcons[i*5 + j].Init(skillID);
                skillScrollIcons[i*5 + j].SkillScroll = this;
            }
        }
    }


    public void Highlight(int skillid)
    {
        if(iconSelected) return;
        foreach(SkillScrollIcon icon in skillScrollIcons)
        {
            if(icon.SkillID == skillid)
            {
                icon.HighLight();
            }
            else
            {
                icon.RemoveHighLight();
            }
        }
        skillScrollDescription.Reset();
        skillScrollDescription.SetSkill(skillid);
    }
    
    public void HighlightExit(int skillid)
    {
        if(iconSelected) return;
        foreach(SkillScrollIcon icon in skillScrollIcons)
        {
            if(icon.SkillID == skillid)
            {
                icon.RemoveHighLight();
            }
        }
        skillScrollDescription.Reset();
    }
    
    
    private void RemoveSelectedImg(int skillid)
    {
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                skillScrollIcons[i * 5 + j].Selected.SetActive(false);
                if(skillScrollIcons[i * 5 + j].SkillID == skillid)
                {
                    skillScrollIcons[i * 5 + j].Selected.SetActive(true);
                }
            }
        }
    }
    
    


    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IconSelected
    {
        get => iconSelected;
        set => iconSelected = value;
    }
}
