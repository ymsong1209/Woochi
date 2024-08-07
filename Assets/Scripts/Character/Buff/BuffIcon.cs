using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [FormerlySerializedAs("buffType")] [SerializeField] private BuffEffect buffEffect;
    [SerializeField] private BuffType bufftype;
    [SerializeField] private Animator animator;
   

    // 버프가 활성화되면서 animation작동되어야함
    public void Activate()
    {
        
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
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

    
    #region BuffDescription
    private void SetBuffDescription(TextMeshProUGUI text)
    {
        text.text = "No Child Buffs";
        BaseBuff childBuff = ReturnChildBuffExceptStatandCureBuff();
        
        if (childBuff)
        {
            childBuff.SetBuffDescription(text);
        }
        //스탯 관련 버프는 버프 설명 이어적어야함.
        else
        {
            if (transform.childCount > 0) text.text = "";
            for (int i = 0; i < transform.childCount; i++)
            {
                childBuff = transform.GetChild(i).GetComponent<BaseBuff>();
                childBuff.SetBuffDescription(text);
            }
            
            // 루프가 끝난 후 마지막에 추가된 "/n"을 제거
            if (text.text.EndsWith("\n"))
            {
                text.text = text.text.Substring(0, text.text.Length - 1);
            }
        }
        
    }
    
    

    BaseBuff ReturnChildBuffExceptStatandCureBuff()
    {
        if(buffEffect == BuffEffect.StatStrengthen || buffEffect == BuffEffect.StatWeaken || buffEffect == BuffEffect.DotCureByDamage)
        {
            return null;
        }
        else
        {
            BaseBuff childbuff = transform.GetChild(0).GetComponent<BaseBuff>();
            if (childbuff.BuffEffect == buffEffect) return childbuff;
            return null;
        }
    }
    #endregion

   
    
    #region Getter Setter
    public BuffEffect BuffEffect => buffEffect;
    public BuffType BuffType => bufftype;

    #endregion
}
