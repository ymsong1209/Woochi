using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * ToDo
 * 소환수 추가
 * 소환수 제거
 * 소환수 소환
*/ 
public class AllyCardList : MonoBehaviour
{
    [SerializeField] List<AllyCard> cards;

    private void Start()
    {
        BattleManager.GetInstance.OnCharacterTurnStart += ShowUI;
    }

    /// <summary>
    /// 소환수 목록을 받아 소환수 카드를 초기화
    /// </summary>
    /// <param name="_allies">우치의 소환수 목록(아직 fix된게 없기에 임시임)</param>
    public void Initialize(Formation _allies)
    {
        List<BaseCharacter> characters = _allies.GetCharacters();

        for(int i = 0; i < cards.Count; i++)
        {
            if(i < characters.Count)
            {
                cards[i].Activate(characters[i]);
            }
            else
            {
                cards[i].Deactivate();
            }
        }
    }

    public void UpdateList()
    {
        foreach(var card in cards)
        {
            card.UpdateCard();
        }
    }
    
    public void ShowUI(BaseCharacter _character, bool isEnable = true)
    {
        if (_character.IsMainCharacter) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
}
