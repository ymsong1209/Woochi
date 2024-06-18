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
                Debug.Log("총 크기가 4가 넘습니다");
                return;
            }

            GameObject characterPrefab = Instantiate(prefab, transform);
            BaseCharacter character = characterPrefab.GetComponent<BaseCharacter>();

            character.Initialize();
            character.IsAlly = isAllyFormation;

            // 캐릭터가 처음부터 소환되는 경우와 아닌 경우를 구분한다
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

    /// <summary>
    /// 소환 성공, 실패 반환
    /// </summary>
    /// <param name="_character">소환할 캐릭터</param>
    /// <param name="_index">소환될 위치의 인덱스</param>
    /// <returns></returns>
    public bool Summon(BaseCharacter _character, int _index)
    {
        if(totalSize + _character.Size > 4)
        {
            Debug.Log("총 크기가 4가 넘습니다");
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
        ReOrder();

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
        ReOrder();
    }

    private void SetProperty(BaseCharacter _character, bool isSummoned, int rowOrder)
    {
        _character.isSummoned = isSummoned;
        _character.RowOrder = rowOrder;
        _character.gameObject.SetActive(isSummoned);
    }
}
