using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BattleManager : SingletonMonobehaviour<GameManager>
{
    private bool                isInCombat = false;
    private BattleState         CurState;
    private BaseCharacter       currentCharacter;

    /// <summary>
    /// 아군이랑 적군을 CharacterList에 채움
    /// </summary>
    [SerializeField] private Queue<BaseCharacter> CharacterQueue = new Queue<BaseCharacter>();

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
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        #region CharacterQueue 초기화
        CharacterQueue.Clear();
        #endregion

        #region 아군과 적군 배치
        // DungeonInfo내에 있는 적들을 알맞은 곳에 배치
        foreach (BaseCharacter enemy in dungeon.EnemyList)
        {
            CharacterQueue.Enqueue(enemy);
        }
        // 아군 캐릭터 알맞게 배치
        foreach(BaseCharacter ally in GameManager.GetInstance.Allies)
        {
            CharacterQueue.Enqueue(ally);
        }

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
        CurState = BattleState.Prebattle;
        CheckPreBuffs();
        DetermineOrder();
         PostTurn,      
         VictoryCheck,  
         PostBattle,    
         END
    }

    /// <summary>
    /// 라운드가 시작할때 적용되는 버프 정리
    /// </summary>
    void CheckPreBuffs()
    {
        int characterCount = CharacterQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue에서 항목을 제거
            BaseCharacter character = CharacterQueue.Dequeue();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundStartBuff();
                //죽음 체크
                character.CheckDead();
            }

            // 수정된 character를 Queue의 뒤쪽에 다시 추가합니다.
            CharacterQueue.Enqueue(character);
        }

    }

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        SortBattleOrder();
        CharacterTurn();
    }

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void SortBattleOrder()
    {
        List<BaseCharacter> SortList = new List<BaseCharacter>();

        // Queue에 있는 요소 하나씩 꺼내서 List에 집어넣음
        foreach (BaseCharacter character in CharacterQueue)
        {
            SortList.Add(character);
        }

        // List 내부의 characters를 Speed 속성을 기준으로 내림차순 정렬
        SortList.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // Queue를 비우고
        CharacterQueue.Clear();

        // 정렬된 List 내부의 characters를 다시 CharacterQueue에 집어넣음
        foreach (BaseCharacter character in SortList)
        {
            CharacterQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 각 캐릭터별로 행동
    /// </summary>
    void CharacterTurn()
    {
        while(CharacterQueue.Count > 0)
        {
            currentCharacter = CharacterQueue.Dequeue();
            //캐릭터가 죽었을 경우에는 버프 체크 후 continue;
            if (currentCharacter.IsDead) {

                continue;
            }
            currentCharacter.
        }
    }
}
