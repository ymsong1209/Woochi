using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<GameManager>
{
    private bool isInCombat = false;
    private BattleState CurState;

    /// <summary>
    /// �Ʊ��̶� ������ CharacterList�� ä��
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
    /// DungeonInfoSO ������ �޾ƿͼ� �Ʊ��� ���� ��ġ�� ����
    /// </summary>
    void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) Debug.LogError("Null Dungeon"); return;

        #region CharacterList �ʱ�ȭ
        CharacterList.Clear();
        #endregion

        #region �Ʊ��� ���� ��ġ
        foreach(BaseCharacter enemy in dungeon.EnemyList)
        {
            
        }
        // DungeonInfo���� �ִ� ������ �˸��� ���� ��ġ

        // �Ʊ� ĳ���� �˸°� ��ġ
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

    }
}
