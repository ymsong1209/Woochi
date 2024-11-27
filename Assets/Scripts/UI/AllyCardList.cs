using System;
using System.Collections.Generic;
using UnityEngine;

public class AllyCardList : MonoBehaviour
{
    [SerializeField] List<AllyCard> cards;
    [SerializeField] AllyCardUI allyCardUI;
    
    [HideInInspector] public bool canSummon = false;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        #region Event Register
        for(int i = 0; i < cards.Count; i++)
        {
            cards[i].OnUIEvent += ProcessUIEvent;
        }
        
        allyCardUI.OnSummon += SummonCard;
        #endregion
    }

    private void OnDisable()
    {
        allyCardUI.HideTooltip();
    }

    /// <summary>
    /// 소환수 목록을 받아 소환수 카드를 초기화
    /// </summary>
    public void Initialize(AllyFormation allies)
    {
        List<BaseCharacter> characters = allies.GetAllies();

        for(int i = 0; i < cards.Count; i++)
        {
            if(i < characters.Count)
            {
                cards[i].Show(characters[i]);
            }
        }
    }
    public void SelectCard(AllyCard card)
    {
        if(!canSummon) return;
        
        allyCardUI.ShowPopup(card);
    }
    
    public void SummonCard(AllyCard card)
    {
        if(card == null) return;
        
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        MC_Summon summon = mainCharacter.SummonSkill;
        BattleManager.GetInstance.SkillSelected(summon);

        if (card.Ally.isSummoned)
        {
            summon.isSummon = false;
            BattleManager.GetInstance.UnSummon(card.Ally);
        }
        else
        {
            summon.isSummon = true;
            summon.willSummon = card.Ally;
        }
    }
    private void ProcessUIEvent(AllyCard allyCard, UIEvent uiEvent)
    {
        if (allyCard.Ally == null) return;
        
        switch (uiEvent)
        {
            case UIEvent.MouseEnter:
                allyCard.ShowAnimation(true);
                allyCardUI.ShowTooltip(allyCard);
                break;
            case UIEvent.MouseExit:
                allyCard.ShowAnimation(false);
                allyCardUI.HideTooltip();
                break;
            case UIEvent.MouseClick:
                SelectCard(allyCard);
                break;
        }
    }
}
