using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<GameManager>
{
    private bool isInCombat = false;
    private BattleState CurState;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (isInCombat == false) return;

        
    }

    void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) Debug.LogError("Null Dungeon"); return;

        // DungeonInfo���� �ִ� ������ �˸��� ���� ��ġ
        foreach(BaseCharacter character in dungeon.Enemy)
        {
            int a = 0;
        }

        // �Ʊ� ĳ���� �˸°� ��ġ

    }
}
