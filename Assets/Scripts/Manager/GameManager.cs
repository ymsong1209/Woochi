using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
   
    [HeaderTooltip("GAME STATE", "Game State�� Inspector���� ���� �Ұ�")]
    [SerializeField,ReadOnly] private GameState gameState;

    void Start()
    {
        gameState = GameState.SELECTROOM;
    }

   
    void Update()
    {
        
    }



}
