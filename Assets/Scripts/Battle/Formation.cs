using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    public BaseCharacter[] formation = new BaseCharacter[4];

    protected List<BaseCharacter> allCharacter = new List<BaseCharacter>();
    [SerializeField] protected float[] singlePos = new float[4];
    [SerializeField] protected float[] multiPos = new float[3];

    [SerializeField] protected bool isAllyFormation;
    [SerializeField] protected int totalSize = 0;

    /// <summary>
    /// 프리펩 리스트를 받아 formation을 초기화한다
    /// </summary>
    /// <param name="prefabs"></param>
    public virtual void Initialize(List<GameObject> prefabs)
    {
        for(int i = 0; i < formation.Length; i++)
        {
            formation[i] = null;
        }

        //int order = 0;
        totalSize = 0;
        foreach (GameObject prefab in prefabs)
        {
            if(totalSize > 4)
            {
                Debug.Log("총 크기가 4가 넘습니다");
                return;
            }

            GameObject characterPrefab = Instantiate(prefab, transform);
            BaseCharacter character = characterPrefab.GetComponent<BaseCharacter>();

            character.IsAlly = isAllyFormation;
            character.RowOrder = totalSize;
            character.Initialize();
            allCharacter.Add(character);

            for (int i = 0; i < character.Size; i++)
            {
                formation[totalSize++] = character;
            }
        }

        Positioning();
    }

    public void CheckDeathInFormation()
    {
        //모든 character에 대해 체력이 0이하인지 검사 후, 죽었으면 formation을 null로 채움.
        //괴목 우두머리 쫄처럼 부활 메커니즘을 가진 몹들은 IsDead로 처리한 캐릭터들도 있기 때문에 체력으로 판별.
       
        foreach (BaseCharacter character in formation)
        {
            if (character && character.Health.CheckHealthZero())
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
        HashSet<BaseCharacter> processedCharacters = new HashSet<BaseCharacter>();
        totalSize = 0;
        foreach (BaseCharacter character in formation)
        {
            if (character != null && !processedCharacters.Contains(character))
            {
                totalSize += character.Size;
                processedCharacters.Add(character);
            }
        }
    }
    
    /// <summary>
    /// 턴이 끝난 후, 바뀐 RowOrder 값을 기반으로 formation을 정렬 후 재배치한다
    /// 캐릭터들을 앞으로 당긴다
    /// </summary>
    public virtual void ReOrder()
    {
        Array.Sort(formation, (character1, character2) => {
            if (character1 == null && character2 == null)
                return 0;
            else if (character1 == null)
                return 1;
            else if (character2 == null)
                return -1;

            return character1.RowOrder.CompareTo(character2.RowOrder);
        });
        
        SetRowOrder();
        Positioning();
    }

    /// <summary>
    /// formation에 있는 캐릭터들을 배치한다.
    /// 캐릭터의 스프라이트 크기를 기반으로 배치한다.
    /// 만약 특수한 배치가 필요하다면, 그때 가서 추가
    /// </summary>
    public void Positioning()
    {
        float moveX;

        for (int index = 0; index < formation.Length;)
        {
            if (formation[index] == null) return;

            BaseCharacter character = formation[index];

            if(character.Size == 1)
            {
                moveX = singlePos[index];
            }
            else
            {
                moveX = multiPos[index];
            }

            character.transform.DOLocalMoveX(moveX, 0.5f);

            index += character.Size;
        }
    }

    /// <summary>
    /// 캐릭터가 formation에서 몇번째 열에 있는지 0~3 사이로 반환
    /// formation내에 없으면 -1 반환
    /// </summary>
    public int FindCharacterIndex(BaseCharacter _character)
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

    private void CleanUp()
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

        allCharacter.Clear();
    }

    public virtual void BattleEnd()
    {
        CleanUp();
    }

    private void SetRowOrder()
    {
        for(int i = 0; i < formation.Length;)
        {
            if (formation[i] == null) break;
            
            formation[i].CheckSkillsOnTurnStart();
            formation[i].RowOrder = i;
            i += formation[i].Size;
        }
    }
    
    public List<BaseCharacter> AllCharacter => allCharacter;
    public float[] SinglePos => singlePos;
    public float[] MultiPos => multiPos;
}
