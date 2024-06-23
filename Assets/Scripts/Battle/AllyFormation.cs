using System.Collections.Generic;
using UnityEngine;

public class AllyFormation : Formation
{
    public List<BaseCharacter> waitingCharacter = new List<BaseCharacter>();

    public override void Initialize(List<GameObject> prefabs)
    {
        for (int i = 0; i < formation.Length; i++)
        {
            formation[i] = null;
        }

        int order = 0;

        foreach (GameObject prefab in prefabs)
        {
            if (totalSize > 4)
            {
                Debug.Log("ì´ í¬ê¸°ê°€ 4ê°€ ë„˜ìŠµë‹ˆë‹¤");
                return;
            }

            GameObject characterPrefab = Instantiate(prefab, transform);
            BaseCharacter character = characterPrefab.GetComponent<BaseCharacter>();

            character.Initialize();
            character.IsAlly = isAllyFormation;

            // ìºë¦­í„°ê°€ ì²˜ìŒë¶€í„° ì†Œí™˜ë˜ëŠ” ê²½ìš°ì™€ ì•„ë‹Œ ê²½ìš°ë¥¼ êµ¬ë¶„í•œë‹¤
            if (character.isStarting)
            {
                SetProperty(character, true, order++);
                character.TriggerBuff(BuffTiming.BattleStart);

                for (int i = 0; i < character.Size; i++)
                {
                    formation[totalSize++] = character;
                }
            }
            else
            {
                SetProperty(character, false, -1);
                waitingCharacter.Add(character);
            }
        }

        Positioning();
    }

    public override void ReOrder()
    {
        base.ReOrder();

        // formation¿¡ µé¾î¿ÍÀÖ´Â »óÅÂÀÎµ¥ °ÔÀÓ ¿ÀºêÁ§Æ®°¡ ºñÈ°¼ºÈ­µÇ¾î ÀÖ´Ù´Â °ÍÀº
        // ¿ìÄ¡°¡ Ä³¸¯ÅÍ¸¦ ¼ÒÈ¯ÇßÀ½À» ÀÇ¹Ì
        for(int i = 0; i < formation.Length;)
        {
            var character = formation[i];
            if (character == null) break;

            if(!character.gameObject.activeSelf)
            {
                character.gameObject.SetActive(true);
                break;
            }    

            i += character.Size;
        }

        // ´ë±â¿­ Ä³¸¯ÅÍ¿¡ µé¾î¿ÍÀÖ´Âµ¥ °ÔÀÓ ¿ÀºêÁ§Æ®°¡ È°¼ºÈ­µÇ¾î ÀÖ´Ù´Â °ÍÀº ¿ìÄ¡°¡ Ä³¸¯ÅÍ¸¦ ¼ÒÈ¯ ÇØÁ¦ÇßÀ½À» ÀÇ¹Ì
        foreach(var character in waitingCharacter)
        {
            if (character.gameObject.activeSelf)
            {
                character.gameObject.SetActive(false);
                break;
            }
        }
    }

    /// <summary>
    /// ì†Œí™˜ ì„±ê³µ, ì‹¤íŒ¨ ë°˜í™˜
    /// </summary>
    /// <param name="_character">ì†Œí™˜í•  ìºë¦­í„°</param>
    /// <param name="_index">ì†Œí™˜ë  ìœ„ì¹˜ì˜ ì¸ë±ìŠ¤</param>
    /// <returns></returns>
    public bool Summon(BaseCharacter _character, int _index)
    {
        if(totalSize + _character.Size > 4)
        {
            Debug.Log("ì´ í¬ê¸°ê°€ 4ê°€ ë„˜ìŠµë‹ˆë‹¤");
            return false;
        }

        waitingCharacter.Remove(_character);

        int rowOrder = formation[_index].RowOrder;

        for(int i = _index; i < formation.Length; i++)
        {
            if (formation[i])
            {
                formation[i].RowOrder++;
            }
            else
            {
                for(int size = 0; size < _character.Size; size++)
                {
                    formation[i++] = _character;
                }

                break;
            }
        }

        totalSize += _character.Size;
        SetProperty(_character, true, rowOrder);

        return true;
    }

    public void UnSummon(BaseCharacter _character)
    {
        waitingCharacter.Add(_character);
        
        for(int i = 0; i < 4; i++)
        {
            if (formation[i] == _character)
            {
                for(int size = 0; size < _character.Size; size++)
                {
                    formation[i + size] = null;
                }
                break;
            }
        }

        totalSize -= _character.Size;
        SetProperty(_character, false, -1);
    }

    private void SetProperty(BaseCharacter _character, bool isSummoned, int rowOrder)
    {
        _character.isSummoned = isSummoned;
        _character.RowOrder = rowOrder;
    }
}
