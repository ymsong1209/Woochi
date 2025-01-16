using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [FormerlySerializedAs("buffType")] [SerializeField] private BuffEffect buffEffect;
    [SerializeField] private BuffType bufftype;
    [SerializeField] private Animator animator;
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform icontransform;
    private bool canInteract;

    private void Awake()
    {
        BattleManager.GetInstance.OnBattleStart += OnBattleStartTriggered;
        icon.color = new Color(1, 1, 1, 0);
        canInteract = true;
    }
    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += () => SetCanInteract(false);
        BattleManager.GetInstance.OnSkillExecuteFinished += () => SetCanInteract(true);
    }

    private void OnDestroy()
    {
        BattleManager.GetInstance.OnBattleStart -= OnBattleStartTriggered;
        BattleManager.GetInstance.OnFocusEnd -= OnFocusEndTriggered;
    }

    private void SetCanInteract(bool value)
    {
        canInteract = value;
    }

    public void OnBattleStartTriggered()
    {
        if (gameObject.activeSelf)
        {
            Activate();
        }
    }

    public void OnFocusEndTriggered()
    {
        Activate();
    }
    public void Activate()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(icontransform.DOScale(1.3f, 0.3f));
        sequence.Join(icon.DOFade(1f, 0.3f));
        sequence.Append(icontransform.DOScale(1f, 0.3f));
        sequence.onComplete += () =>
        {
            BattleManager.GetInstance.OnFocusEnd -= OnFocusEndTriggered;
        };
    }

    public void DeActivate()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(icon.rectTransform.DOScale(1.3f, 0.3f));
        sequence.Append(icon.rectTransform.DOScale(1f, 0.3f));
        sequence.Join(icon.DOFade(0f, 0.3f));
        sequence.OnComplete(() => gameObject.SetActive(false));
        //gameObject.SetActive(false);
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canInteract) return;
        TextMeshProUGUI text = UIManager.GetInstance.BuffPopupUI.PopUpText;
        SetBuffDescription(text);
        UIManager.GetInstance.ActivateBuffPopUp(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.DeactivateBuffPopUp();
    }

    
    #region BuffDescription
    private void SetBuffDescription(TextMeshProUGUI text)
    {
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
        if(buffEffect == BuffEffect.StatStrengthen || buffEffect == BuffEffect.StatWeaken || buffEffect == BuffEffect.DotCureByDamage ||
           buffEffect == BuffEffect.ElementalStatStrengthen || buffEffect == BuffEffect.ElementalStatWeaken ||
           buffEffect == BuffEffect.Special)
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
