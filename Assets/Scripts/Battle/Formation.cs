using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    public BaseCharacter[] formation = new BaseCharacter[4];

    [SerializeField] private bool isAllyFormation;

    /// <summary>
    /// ������ ����Ʈ�� �޾� formation�� �ʱ�ȭ�Ѵ�
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

    /// <summary>
    /// ���� ���� ��, �ٲ� RowOrder ���� ������� formation�� ���� �� ���ġ�Ѵ�
    /// ĳ���͵��� ������ ����
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
    /// formation�� �ִ� ĳ���͵��� ��ġ�Ѵ�.
    /// ĳ������ ��������Ʈ ũ�⸦ ������� ��ġ�Ѵ�.
    /// ���� Ư���� ��ġ�� �ʿ��ϴٸ�, �׶� ���� �߰�
    /// </summary>
    public void Positioning()
    {
        float direction = isAllyFormation ? -1f : 1f;
        float moveX = 0f;

        for (int index = 0; index < formation.Length;)
        {
            if (formation[index] == null) return;

            BaseCharacter character = formation[index];

            float radius = character.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            moveX += (radius * direction);

            character.transform.DOLocalMoveX(moveX, 0.5f);
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
}
