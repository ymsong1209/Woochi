using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private Animator animator;
   

    // 버프가 활성화되면서 animation작동되어야함
    public void Activate()
    {
        
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void CheckChildBuffs(BaseBuff _buff)
    {
        // Check if buffType is not statstrengthen or statweaken
        if (buffType != BuffType.StatStrengthen && buffType != BuffType.StatWeaken)
        {
            //부모 자식 연결 해제
            _buff.transform.parent = null;
            DeActivate();
        }
        else
        {
            if (_buff.BuffType == BuffType.StatStrengthen)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    BaseBuff childBuff = transform.GetChild(i).GetComponent<BaseBuff>();
                    StatBuff childStatStrengthenBuff = childBuff as StatBuff;
                    StatBuff statStrengthenBuff = _buff as StatBuff;
                    // child가 statstrengthen인지 확인
                    if (statStrengthenBuff && childStatStrengthenBuff 
                                           && statStrengthenBuff.StatBuffName == childStatStrengthenBuff.StatBuffName)
                    {
                        childStatStrengthenBuff.transform.parent = null;
                    }
                }
            }
            else if (_buff.BuffType == BuffType.StatWeaken)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    BaseBuff childBuff = transform.GetChild(i).GetComponent<BaseBuff>();
                    StatDeBuff childStatWeakenBuff = childBuff as StatDeBuff;
                    StatDeBuff statWeakenBuff = _buff as StatDeBuff;
                    // child가 statweaken인지 확인
                    if (statWeakenBuff && childStatWeakenBuff 
                                       && statWeakenBuff.StatBuffName == childStatWeakenBuff.StatBuffName)
                    {
                        childStatWeakenBuff.transform.parent = null;
                    }
                }
            }
            
            if (transform.childCount == 0)
            {
                DeActivate();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TextMeshProUGUI text = UIManager.GetInstance.BuffPopupUI.PopUpText;
        SetBuffDescription(text);
        UIManager.GetInstance.ActivateBuffPopUp(Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.DeactivateBuffPopUp();
    }

    private void SetBuffDescription(TextMeshProUGUI text)
    {
        switch (buffType)
        {
            case BuffType.Bleed:
                SetBleedDescription(text);
                break;
            case BuffType.Burn:
                SetBurnDescription(text);
                break;
            case BuffType.Stun:
                //return "This buff stuns the character.";
            case BuffType.StatWeaken:
                SetStatWeakenDescription(text);
                break;
            case BuffType.StatStrengthen:
                SetStatStrengthDescription(text);
                break;
            case BuffType.HealOverTime:
                //return "This buff heals the character over time.";
            case BuffType.Protect:
                //return "This buff protects the character.";
            case BuffType.Shield:
                //return "This buff shields the character.";
            default:
                //return "Unknown buff type.";
                break;
        }
    }
    
    #region BuffDescription

    BaseBuff ReturnChildBuff()
    {
        if(buffType == BuffType.StatStrengthen || buffType == BuffType.StatWeaken)
        {
            return null;
        }
        else
        {
            BaseBuff childbuff = transform.GetChild(0).GetComponent<BaseBuff>();
            if (childbuff.BuffType == buffType) return childbuff;
            return null;
        }
    }
    private void SetBleedDescription(TextMeshProUGUI text)
    {
        BaseBuff childBuff = ReturnChildBuff();
        if (childBuff)
        {
            BleedDeBuff bleedDeBuff = childBuff as BleedDeBuff;
            string description = "출혈" +bleedDeBuff.BuffDurationTurns+ " : 매턴마다 최대 체력의 " + bleedDeBuff.BleedPercent + "% 만큼 피해를 입습니다.";
            text.text = description;
            text.color = Color.red;
        }
    }
    
    private void SetBurnDescription(TextMeshProUGUI text)
    {
        BaseBuff childBuff = ReturnChildBuff();
        if (childBuff)
        {
            BurnDebuff burnDebuff = childBuff as BurnDebuff;
            string description = "화상" +burnDebuff.BuffDurationTurns+ " : 매턴마다 최대 체력의 5% 만큼 피해를 입습니다.";
            text.text = description;
            text.color = Color.red;
        }
    }
    
    private void SetStatStrengthDescription(TextMeshProUGUI text)
    {
        string description = "";
        for (int i = 0; i < transform.childCount; i++)
        {
            BaseBuff childBuff = transform.GetChild(i).GetComponent<BaseBuff>();

            // child가 statstrengthen인지 확인
            if (childBuff && childBuff.BuffType == BuffType.StatStrengthen)
            {
                // Cast the BaseBuff to StatStrengthenBuff to access its specific properties
                StatBuff statStrengthenBuff = childBuff as StatBuff;

                // Set the description text
                description += statStrengthenBuff.StatBuffName + statStrengthenBuff.BuffDurationTurns;
                if (statStrengthenBuff.ChangeDefense > 0)
                {
                    description += "방어력 : +" + statStrengthenBuff.ChangeDefense + " ";
                }
                if (statStrengthenBuff.ChangeCrit > 0)
                {
                    description += "치명타 : +" + statStrengthenBuff.ChangeCrit + " ";
                }
                if (statStrengthenBuff.ChangeAccuracy > 0)
                {
                    description += "명중 : +" + statStrengthenBuff.ChangeAccuracy + " ";
                }
                if (statStrengthenBuff.ChangeEvasion > 0)
                {
                    description += "회피 : +" + statStrengthenBuff.ChangeEvasion + " ";
                }
                if (statStrengthenBuff.ChangeResist > 0)
                {
                    description += "저항 : +" + statStrengthenBuff.ChangeResist + " ";
                }
                if (statStrengthenBuff.ChangeMinStat > 0)
                {
                    description += "최소 스탯 : +" + statStrengthenBuff.ChangeMinStat + " ";
                }
                if (statStrengthenBuff.ChangeMaxStat > 0)
                {
                    description += "최대 스탯 : +" + statStrengthenBuff.ChangeMaxStat + " ";
                }
                if (statStrengthenBuff.ChangeSpeed > 0)
                {
                    description += "속도 : +" + statStrengthenBuff.ChangeSpeed + " ";
                }
                description += "\n";
            }
        }
        text.text = description;
        text.color = Color.blue;
    }
    
    private void SetStatWeakenDescription(TextMeshProUGUI text)
    {
        string description = "";
        for (int i = 0; i < transform.childCount; i++)
        {
            BaseBuff childBuff = transform.GetChild(i).GetComponent<BaseBuff>();

            // child가 statweaken인지 확인
            if (childBuff && childBuff.BuffType == BuffType.StatWeaken)
            {
                // Cast the BaseBuff to StatStrengthenBuff to access its specific properties
                StatDeBuff statStrengthenBuff = childBuff as StatDeBuff;

                // Set the description text
                description += statStrengthenBuff.StatBuffName + statStrengthenBuff.BuffDurationTurns+": ";
                if (statStrengthenBuff.ChangeDefense < 0)
                {
                    description += "방어력 : " + statStrengthenBuff.ChangeDefense + " ";
                }
                if (statStrengthenBuff.ChangeCrit < 0)
                {
                    description += "치명타 : " + statStrengthenBuff.ChangeCrit + " ";
                }
                if (statStrengthenBuff.ChangeAccuracy < 0)
                {
                    description += "명중 : " + statStrengthenBuff.ChangeAccuracy + " ";
                }
                if (statStrengthenBuff.ChangeEvasion < 0)
                {
                    description += "회피 : " + statStrengthenBuff.ChangeEvasion + " ";
                }
                if (statStrengthenBuff.ChangeResist < 0)
                {
                    description += "저항 : " + statStrengthenBuff.ChangeResist + " ";
                }
                if (statStrengthenBuff.ChangeMinStat < 0)
                {
                    description += "최소 스탯 : " + statStrengthenBuff.ChangeMinStat + " ";
                }
                if (statStrengthenBuff.ChangeMaxStat < 0)
                {
                    description += "최대 스탯 : " + statStrengthenBuff.ChangeMaxStat + " ";
                }
                if (statStrengthenBuff.ChangeSpeed < 0)
                {
                    description += "속도 : " + statStrengthenBuff.ChangeSpeed + " ";
                }
                description += "\n";
            }
        }
        text.text = description;
        text.color = Color.red;
    }
    #endregion

   
    
    #region Getter Setter
    public BuffType BuffType => buffType;
    // public BuffType SkillOwner
    // {
    //     get { return skillOwner; }
    //     set { skillOwner = value; }
    // }
    #endregion
}
