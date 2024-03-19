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

        // DungeonInfo내에 있는 적들을 알맞은 곳에 배치
        foreach(BaseCharacter character in dungeon.Enemy)
        {
            int a = 0;
        }

        // 아군 캐릭터 알맞게 배치

    }
}
