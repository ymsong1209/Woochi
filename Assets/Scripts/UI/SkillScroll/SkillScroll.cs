using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillScroll : MonoBehaviour
{
    [SerializeField] private Sprite selectedIconDefault;
    [SerializeField] private Sprite noiconImg;
    [SerializeField] private Image[] selectedIcons = new Image[5];
    [SerializeField] private SkillScrollIcon[] skillScrollIcons = new SkillScrollIcon[25];
    

    public void Activate()
    {
        gameObject.SetActive(true);
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
            }
        }
    }
    
    
    [SerializeField] private 
    // Start is called before the first frame update
    void Start()
    {
        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
