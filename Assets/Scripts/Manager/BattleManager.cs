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
    /// �Ʊ��̶� ������ CharacterList�� ä��
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
    /// DungeonInfoSO ������ �޾ƿͼ� �Ʊ��� ���� ��ġ�� ����
    /// </summary>
    void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        #region CharacterQueue �ʱ�ȭ
        CharacterQueue.Clear();
        #endregion

        #region �Ʊ��� ���� ��ġ
        // DungeonInfo���� �ִ� ������ �˸��� ���� ��ġ
        foreach (BaseCharacter enemy in dungeon.EnemyList)
        {
            CharacterQueue.Enqueue(enemy);
        }
        // �Ʊ� ĳ���� �˸°� ��ġ
        foreach(BaseCharacter ally in GameManager.GetInstance.Allies)
        {
            CharacterQueue.Enqueue(ally);
        }

        #endregion

        #region PreBattle ���·� �Ѿ
        PreBattle();
        #endregion

    }

    /// <summary>
    /// ĳ���͵��� ���� ����
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
    /// ���尡 �����Ҷ� ����Ǵ� ���� ����
    /// </summary>
    void CheckPreBuffs()
    {
        int characterCount = CharacterQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue���� �׸��� ����
            BaseCharacter character = CharacterQueue.Dequeue();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundStartBuff();
                //���� üũ
                character.CheckDead();
            }

            // ������ character�� Queue�� ���ʿ� �ٽ� �߰��մϴ�.
            CharacterQueue.Enqueue(character);
        }

    }

    /// <summary>
    /// ĳ���͵��� �ӵ������� ����
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        SortBattleOrder();
        CharacterTurn();
    }

    /// <summary>
    /// ĳ���͵��� �ӵ������� ����
    /// </summary>
    void SortBattleOrder()
    {
        List<BaseCharacter> SortList = new List<BaseCharacter>();

        // Queue�� �ִ� ��� �ϳ��� ������ List�� �������
        foreach (BaseCharacter character in CharacterQueue)
        {
            SortList.Add(character);
        }

        // List ������ characters�� Speed �Ӽ��� �������� �������� ����
        SortList.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // Queue�� ����
        CharacterQueue.Clear();

        // ���ĵ� List ������ characters�� �ٽ� CharacterQueue�� �������
        foreach (BaseCharacter character in SortList)
        {
            CharacterQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// �� ĳ���ͺ��� �ൿ
    /// </summary>
    void CharacterTurn()
    {
        while(CharacterQueue.Count > 0)
        {
            currentCharacter = CharacterQueue.Dequeue();
            //ĳ���Ͱ� �׾��� ��쿡�� ���� üũ �� continue;
            if (currentCharacter.IsDead) {

                continue;
            }
            currentCharacter.
        }
    }
}
