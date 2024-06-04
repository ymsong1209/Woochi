using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
   
    [HeaderTooltip("GAME STATE", "Game State는 Inspector에서 수정 불가")]
    [SerializeField,ReadOnly] private GameState gameState;
    public DungeonInfoSO temporaryDungeon;
    [SerializeField] private List<GameObject> allies = new List<GameObject>();


    [SerializeField] private BaseCharm[] charmList = new BaseCharm[5];

    void Start()
    {
        SelectRoom();
        
    }

   
    void Update()
    {
        if(gameState == GameState.SELECTROOM)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameState = GameState.BATTLE;
                Debug.Log("GameState : Battle");
                BattleManager.GetInstance.InitializeBattle(temporaryDungeon);
            }
        }
    }

    public void SelectRoom()
    {
        gameState = GameState.SELECTROOM;
        Debug.Log("GameState : SelectRoom");
    }
    
    public void RemoveCharm(BaseCharm charm)
    {
        for(int i = 0;i<charmList.Length;++i)
        {
            if(charmList[i] == charm)
            {
                charmList[i] = null;
                break;
            }
        }
    }


    #region Getter Setter
    public List<GameObject> Allies => allies;
    public BaseCharm[] CharmList => charmList;
    #endregion
}
