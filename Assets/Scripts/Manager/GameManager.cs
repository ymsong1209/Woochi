using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
   
    [HeaderTooltip("GAME STATE", "Game State는 Inspector에서 수정 불가")]
    [SerializeField,ReadOnly] private GameState gameState;

    [SerializeField] private List<GameObject> allies = new List<GameObject>();

    public DungeonInfoSO temporaryDungeon;

    void Start()
    {
        gameState = GameState.SELECTROOM;
        foreach (GameObject go in allies)
        {
            BaseAllyCharacter character = go.GetComponent<BaseAllyCharacter>();
            character.Initialize();
        }
    }

   
    void Update()
    {
        if(gameState == GameState.SELECTROOM)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BattleManager.GetInstance.InitializeBattle(temporaryDungeon);
            }
        }
    }

    #region Getter Setter
    public List<GameObject> Allies => allies;
    #endregion
    public void AddAlly(GameObject ally)
    {
        if (!allies.Contains(ally))
        {
            allies.Add(ally);
        }
    }

    public void RemoveAlly(GameObject ally)
    {
        if (allies.Contains(ally))
        {
            allies.Remove(ally);
        }
    }
}
