using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    public BaseCharacter[] formation = new BaseCharacter[4];

    [SerializeField] private bool isAllyFormation;

    /// <summary>
    /// 프리펩 리스트를 받아 formation을 초기화한다
    /// </summary>
    /// <param name="prefabs"></param>
    public void Initialize(List<GameObject> prefabs)
    {
        int size = 0;

        for(int i = 0; i < formation.Length; i++)
        {
            formation[i] = null;
        }

        foreach (GameObject prefab in prefabs)
        {
            GameObject characterPrefab = Instantiate(prefab, transform);
            BaseCharacter character = characterPrefab.GetComponent<BaseCharacter>();

            character.Initialize();
            character.IsAlly = isAllyFormation;

            character.rowOrder = size;

            character.ApplyBuff(BuffTiming.BattleStart);

            for (int i = 0; i < character.Size; i++)
            {
                formation[size++] = character;
            }
        }

        Positioning();
    }

    public void CheckDeathInFormation()
    {
        //모든 character에 대해 checkdead검사 후, 죽었으면 formation을 null로 채움. 
        foreach (BaseCharacter character in formation)
        {
            if (character && character.CheckDead())
            {
                for (int i = 0; i < formation.Length; i++)
                {
                    if (formation[i] == character)
                    {
                        formation[i] = null; // 자기 자신을 null로 설정
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 턴이 끝난 후, 바뀐 RowOrder 값을 기반으로 formation을 정렬 후 재배치한다
    /// 캐릭터들을 앞으로 당긴다
    /// </summary>
    public void ReOrder()
    {
        Array.Sort(formation, (character1, character2) => {
            if (character1 == null && character2 == null)
                return 0;
            else if (character1 == null)
                return 1;
            else if (character2 == null)
                return -1;

            return character1.rowOrder.CompareTo(character2.rowOrder);
        });

        Positioning();
    }

    /// <summary>
    /// formation에 있는 캐릭터들을 배치한다.
    /// 캐릭터의 스프라이트 크기를 기반으로 배치한다.
    /// 만약 특수한 배치가 필요하다면, 그때 가서 추가
    /// </summary>
    public void Positioning()
    {
        float direction = isAllyFormation ? -1f : 1f;
        float moveX = transform.position.x;

        for (int index = 0; index < formation.Length;)
        {
            if (formation[index] == null) return;

            BaseCharacter character = formation[index];

            character.transform.DOLocalMoveX(moveX, 0.5f);
            character.onAnyTurnEnd?.Invoke();

            float radius = 4.5f;
            moveX += (radius * direction);

            index += character.Size;
        }
    }

    public int FindCharacter(BaseCharacter _character)
    {
        int index = -1;

        for(int i = 0; i < formation.Length; i++)
        {
            if (formation[i] == _character)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    /// <summary>
    /// 이 포메이션에 있는 캐릭터의 정보들(size가 2인 캐릭터 구별 위함)
    /// </summary>
    public List<BaseCharacter> GetCharacters()
    {
        List<BaseCharacter> list = new List<BaseCharacter>();

        for(int size = 0; size < formation.Length;) 
        {
            if (formation[size] == null) break;
            list.Add(formation[size]);
            size += formation[size].Size;
        }

        return list;
    }

    public void CleanUp()
    {
        foreach(BaseCharacter character in formation)
        {
            if (character == null) continue;
            character.Destroy();
            Destroy(character.gameObject);
        }
        //자식 gameobject를 모두 삭제
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
