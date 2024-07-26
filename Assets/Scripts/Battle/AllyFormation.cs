using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllyFormation : Formation
{
    [SerializeField] GameObject dummyPrefab;
    private BaseCharacter dummyCharacter; //우치 소환수 소환용 더미 캐릭터

    public override void Initialize(List<GameObject> prefabs)
    {
        for (int i = 0; i < formation.Length; i++)
        {
            formation[i] = null;
        }

        totalSize = 0;

        // 플레이어가 소유한 소환수를 일단 모두 생성(우치 포함)
        foreach (GameObject prefab in prefabs)
        {
            GameObject characterPrefab = Instantiate(prefab, transform);
            BaseCharacter character = characterPrefab.GetComponent<BaseCharacter>();

            character.IsAlly = isAllyFormation;
            character.Initialize();
            SetProperty(character, false, -1);
            character.gameObject.SetActive(false);
            allCharacter.Add(character);
        }

        // 포메이션에 등록한 소환수를 포메이션에 등록
        int[] battleFormation = DataCloud.playerData.battleData.formation;
        for(int i = 0; i < 4; i++)
        {
            if (battleFormation[i] == -1) continue;

            BaseCharacter character = allCharacter.FirstOrDefault(c => c.ID == battleFormation[i]);
            SetProperty(character, true, i);
            character.gameObject.SetActive(true);
            character.TriggerBuff(BuffTiming.BattleStart);
            for(int s = 0; s < character.Size; s++)
            {
                formation[totalSize++] = character;
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

        // formation에 들어와있는 상태인데 게임 오브젝트가 비활성화되어 있다는 것은
        // 우치가 캐릭터를 소환했음을 의미
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

        // 대기열 캐릭터에 들어와있는데 게임 오브젝트가 활성화되어 있다는 것은 우치가 캐릭터를 소환 해제했음을 의미
        foreach(var character in allCharacter)
        {
            if (character.gameObject.activeSelf && !character.isSummoned)
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

    /// <summary>
    /// 우치 제외 소환수만 반환
    /// </summary>
    public List<BaseCharacter> GetAllies()
    {
        List<BaseCharacter> list = new List<BaseCharacter>();

        foreach(var character in allCharacter)
        {
            if(character.IsMainCharacter) continue;
            list.Add(character);
        }

        return list;
    }

    /// <summary>
    /// 전투에 참여한 캐릭터만 반환
    /// </summary>
    /// <returns></returns>
    public List<BaseCharacter> GetBattleCharacter()
    {
        List<BaseCharacter> list = new List<BaseCharacter>();

        for(int i = 0; i < formation.Length;)
        {
            if (formation[i] == null) break;
            list.Add(formation[i]);
            i += formation[i].Size;
        }

        return list;
    }

    public override void BattleEnd()
    {
        foreach(var character in allCharacter)
        {
            character.TriggerBuff(BuffTiming.BattleEnd);
            character.RemoveAllBuff();
            character.SaveStat();
        }

        SaveFormation();
    }

    private void SaveFormation()
    {
        int[] newFormation = new int[4] { -1, -1, -1, -1 };
        List<BaseCharacter> characters = GetBattleCharacter();

        for (int i = 0; i < characters.Count; i++)
        {
            newFormation[i] = characters[i].ID;
        }
        DataCloud.playerData.battleData.formation = newFormation;
    }

    private void SetProperty(BaseCharacter _character, bool isSummoned, int rowOrder)
    {
        _character.isSummoned = isSummoned;
        _character.RowOrder = rowOrder;
    }
}
