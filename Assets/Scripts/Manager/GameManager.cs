using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
   
    [HeaderTooltip("GAME STATE", "Game State는 Inspector에서 수정 불가")]
    [SerializeField,ReadOnly] private GameState gameState;

    [SerializeField] private List<BaseCharacter> Allies = new List<BaseCharacter>();

    void Start()
    {
        gameState = GameState.SELECTROOM;
    }

   
    void Update()
    {
        
    }



}
