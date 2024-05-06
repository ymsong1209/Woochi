using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * ToDo
 * ��ȯ�� �߰�
 * ��ȯ�� ����
 * ��ȯ�� ��ȯ
*/ 
public class AllyCardList : MonoBehaviour
{
    [SerializeField] List<AllyCard> cards;

    /// <summary>
    /// ��ȯ�� ����� �޾� ��ȯ�� ī�带 �ʱ�ȭ
    /// </summary>
    /// <param name="_allies">��ġ�� ��ȯ�� ���(���� fix�Ȱ� ���⿡ �ӽ���)</param>
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
}
