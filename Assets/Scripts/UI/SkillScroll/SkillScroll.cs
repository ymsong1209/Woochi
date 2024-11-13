using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillScroll : MonoBehaviour
{
    [SerializeField] public Sprite NoiconImg;
    
    //우치가 선택한 스킬들
    [SerializeField] private Sprite selectedIconDefault;
    [SerializeField] private Image[] selectedIcons = new Image[5];
    
    //도술 두루마리에 있는 아이콘들
    [SerializeField] private SkillScrollIcon[] skillScrollIcons = new SkillScrollIcon[25];
    [SerializeField] private bool isIconSelected;
    [SerializeField] private Button skillScrollBackground;
    private int selectedSkillID;
    
    //도술 설명
    [SerializeField] private SkillScrollDescription skillScrollDescription;

    public Action<int> OnIconHovered;
    public Action<int> OnIconHoverExit;
    public Action<int> OnSkillSelected;
    
    void Start()
    {
        Activate();
        OnIconHovered += HoverEnter;
        OnIconHoverExit += HoverExit;
        OnSkillSelected += SkillIconClicked;
        skillScrollBackground.onClick.AddListener(()=>Reset());
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        isIconSelected = false;
        //우치가 선택한 도술 세팅
        for (int i = 0; i < DataCloud.playerData.currentskillIDs.Length; i++)
        {
            int skillID = DataCloud.playerData.currentskillIDs[i];
            //한 속성에 도술이 없는 경우
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
                    selectedIcons[i].sprite = NoiconImg;
                }
                
            }
        }
        
        //도술 두루마리에 가지고 있는 도술 세팅
        for (int i = 0; i < DataCloud.playerData.totalSkillIDs.GetLength(0); i++)
        {
            for (int j = 0; j < DataCloud.playerData.totalSkillIDs.GetLength(1); j++)
            {
                int skillID = DataCloud.playerData.totalSkillIDs[i, j];
                skillScrollIcons[i*5 + j].SkillScroll = this;
                skillScrollIcons[i*5 + j].Init(skillID);
                
            }
        }
    }


    private void HoverEnter(int skillid)
    {
        if (skillid == 0) return;
        skillScrollDescription.Reset();
        skillScrollDescription.SetSkill(skillid);
    }
    
    public void HoverExit(int skillid)
    {
        if (skillid == 0) return;
        skillScrollDescription.Reset();
        if (isIconSelected && selectedSkillID!=0)
        {
            skillScrollDescription.SetSkill(selectedSkillID);
        }
    }

    private void SkillIconClicked(int skillid)
    {
        if (skillid == 0) return;
        
        //동일한 스킬 아이콘 눌렀을 경우 선택 해제
        if(isIconSelected && selectedSkillID == skillid)
        {
            Reset();
            return;
        }
        
        
        isIconSelected = true;
        selectedSkillID = skillid;
        
        //selected 이미지 설정
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
      
        
        skillScrollDescription.Reset();
        skillScrollDescription.SetSkill(skillid);
        
    }

    public void Reset()
    {
        isIconSelected = false;
        selectedSkillID = 0;
        for (int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                skillScrollIcons[i * 5 + j].Selected.SetActive(false);
            }
        }
        skillScrollDescription.Reset();
    }
    
    public bool IsIconSelected
    {
        get => isIconSelected;
        set => isIconSelected = value;
    }
}
