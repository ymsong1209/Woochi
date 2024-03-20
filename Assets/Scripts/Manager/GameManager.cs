using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
   
    [HeaderTooltip("GAME STATE", "Game State는 Inspector에서 수정 불가")]
    [SerializeField,ReadOnly] private GameState gameState;

    [SerializeField] private List<BaseCharacter> allies = new List<BaseCharacter>();

    void Start()
    {
        gameState = GameState.SELECTROOM;
    }

   
    void Update()
    {
       
    }

    #region Getter Setter
    public List<BaseCharacter> Allies => allies;
    #endregion
    public void AddAlly(BaseCharacter ally)
    {
        if (!allies.Contains(ally))
        {
            allies.Add(ally);
        }
    }

    public void RemoveAlly(BaseCharacter ally)
    {
        if (allies.Contains(ally))
        {
            allies.Remove(ally);
        }
    }
}
