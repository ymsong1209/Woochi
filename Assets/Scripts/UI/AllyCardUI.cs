using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyCardUI : MonoBehaviour
{
    [Header("Tooltip")]
    [SerializeField] private GameObject tooltip;
    [SerializeField] private TextMeshProUGUI tooltipNameText;
    [SerializeField] private TextMeshProUGUI tooltipStatusText;

    [Header("Popup")] 
    [SerializeField] private GameObject popup;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Button yesBtn;
    [SerializeField] private Button noBtn;
    [SerializeField] private Button confirmBtn;
    
    public Action<AllyCard> OnSummon;
    private AllyCard currentCard;
    
    private void Awake()    
    {
        tooltip.SetActive(false);
        popup.SetActive(false);
    }

    private void Start()
    {
        yesBtn.onClick.AddListener(() => OnSummon?.Invoke(currentCard));
    }

    public void ShowTooltip(AllyCard allyCard)
    {
        BaseCharacter ally = allyCard.Ally;
        tooltipNameText.text = $"[{ally.Name}]";
        
        string allyStatus = string.Empty;
    
        if (ally.IsDead)
        {
            allyStatus = $"부활까지 {ally.Health.TurnToResurrect}번의 지점 남음";
        }
        else
        {
            if (ally.isSummoned)
            {
                allyStatus = "소환 중";
            }
            else
            {
                allyStatus = "소환 가능";
            }
        }
        tooltipStatusText.text = allyStatus;
        tooltip.SetActive(true);
    }
    
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void ShowPopup(AllyCard allyCard)
    {
        currentCard = allyCard;
        BaseCharacter ally = allyCard.Ally;
        string allyName = ally.Name;

        if (ally.IsDead)
        {
            popupText.text = $"{allyName}은(는) 사망한 상태다";
            ShowButton(false);
        }
        else
        {
            bool isEnable = true;
            
            if (ally.isSummoned)
            {
                popupText.text = $"{allyName}을(를) 소환 해제할까?";
            }
            else
            {
                AllyFormation allies = BattleManager.GetInstance.Allies;

                if (allies.CanSummon(ally))
                {
                    popupText.text = $"{allyName}을(를) 소환할까?\n소환할 위치를 선택하자";
                }
                else
                {
                    popupText.text = "소환할 공간이 부족하다";
                    isEnable = false;
                }
            }
            
            ShowButton(isEnable);
        }
        
        popup.SetActive(true);
    }

    private void ShowButton(bool isEnable)
    {
        yesBtn.gameObject.SetActive(isEnable);
        noBtn.gameObject.SetActive(isEnable);
        confirmBtn.gameObject.SetActive(!isEnable);
    }
}
