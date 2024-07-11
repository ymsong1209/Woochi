using System.Collections.Generic;
using UnityEngine;

public class AllyFormation : Formation
{
    public List<BaseCharacter> waitingCharacter = new List<BaseCharacter>();
    [SerializeField] GameObject dummyPrefab;
    private BaseCharacter dummyCharacter;

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
                return;
            }

            GameObject characterPrefab = Instantiate(prefab, transform);
            BaseCharacter character = characterPrefab.GetComponent<BaseCharacter>();

            character.Initialize();
            character.IsAlly = isAllyFormation;

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
                character.gameObject.SetActive(false);
                waitingCharacter.Add(character);
            }
        }

        #region DummyCharacter
        GameObject dummy = Instantiate(dummyPrefab, transform);
        dummyCharacter = dummy.GetComponent<BaseCharacter>();
        dummyCharacter.gameObject.SetActive(false);
        dummyCharacter.IsAlly = true;
        #endregion

        Positioning();
    }

    public override void ReOrder()
    {
        base.ReOrder();

        // formation�� �����ִ� �����ε� ���� ������Ʈ�� ��Ȱ��ȭ�Ǿ� �ִٴ� ����
        // ��ġ�� ĳ���͸� ��ȯ������ �ǹ�
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

        // ��⿭ ĳ���Ϳ� �����ִµ� ���� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִٴ� ���� ��ġ�� ĳ���͸� ��ȯ ���������� �ǹ�
        foreach(var character in waitingCharacter)
        {
            if (character.gameObject.activeSelf)
            {
                character.gameObject.SetActive(false);
                break;
            }
        }
    }

    public bool Summon(BaseCharacter _character, int _index)
    {
        if(totalSize + _character.Size > 4)
        {
            return false;
        }

        waitingCharacter.Remove(_character);

        int rowOrder = _index;
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

    public void EnableDummy()
    {
        for(int i = 0; i < formation.Length; i++)
        {
            if (formation[i] == null)
            {
                formation[i] = dummyCharacter;
                dummyCharacter.gameObject.SetActive(true);
                dummyCharacter.RowOrder = i;
                ReOrder();
                break;
            }
        }
    }

    public void DisableDummy()
    {
        for(int i = 0; i < formation.Length; i++)
        {
            if (formation[i] && formation[i].isDummy)
            {
                formation[i] = null;
                dummyCharacter.HUD.ActivateArrow(false);
                dummyCharacter.gameObject.SetActive(false);
            }
        }
    }

    private void SetProperty(BaseCharacter _character, bool isSummoned, int rowOrder)
    {
        _character.isSummoned = isSummoned;
        _character.RowOrder = rowOrder;
    }
}
