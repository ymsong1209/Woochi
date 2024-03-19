using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<GameManager>
{
    private bool isInCombat = false;
    private BattleState CurState;

    /// <summary>
    /// 아군이랑 적군을 CharacterList에 채움
    /// </summary>
    [SerializeField] private List<BaseCharacter> CharacterList = new List<BaseCharacter>();

    void Start()
    {
        
    }

    
    void Update()
    {
        if (isInCombat == false) return;

        
    }

    /// <summary>
    /// DungeonInfoSO 정보를 받아와서 아군과 적군 위치값 설정
    /// </summary>
    void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) Debug.LogError("Null Dungeon"); return;

        #region CharacterList 초기화
        CharacterList.Clear();
        #endregion

        #region 아군과 적군 배치
        foreach(BaseCharacter enemy in dungeon.EnemyList)
        {
            
        }
        // DungeonInfo내에 있는 적들을 알맞은 곳에 배치

        // 아군 캐릭터 알맞게 배치
        #endregion

        #region PreBattle 상태로 넘어감
        PreBattle();
        #endregion

    }

    /// <summary>
    /// 캐릭터들의 버프 정리
    /// </summary>
    void PreBattle()
    {

    }
}
