using System.Collections.Generic;
using UnityEngine;

public class AllyCardList : MonoBehaviour
{
    [SerializeField] List<AllyCard> cards;

    [HideInInspector] public bool canSummon = false;

    private void Start()
    {
        BattleManager.GetInstance.ShowCharacterUI += ShowUI;
    }

    /// <summary>
    /// 소환수 목록을 받아 소환수 카드를 초기화
    /// </summary>
    public void Initialize(AllyFormation _allies)
    {
        List<BaseCharacter> characters = _allies.GetAllies();

        for(int i = 0; i < cards.Count; i++)
        {
            if(i < characters.Count)
            {
                cards[i].Show(characters[i]);
            }
        }
    }
    
    public void ShowUI(BaseCharacter _character, bool isEnable = true)
    {
        if (_character.IsMainCharacter) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    public void SelectCard(AllyCard _card)
    {
        if(!canSummon) return;

        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        MC_Summon summon = mainCharacter.SummonSkill;
        BattleManager.GetInstance.SkillSelected(summon);

        if (_card.Ally.isSummoned)
        {
            summon.isSummon = false;
            BattleManager.GetInstance.UnSummon(_card.Ally);
        }
        else
        {
            summon.isSummon = true;
            summon.willSummon = _card.Ally;
        }
    }
}
