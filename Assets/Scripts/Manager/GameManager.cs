using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
   
    [HeaderTooltip("GAME STATE", "Game State�� Inspector���� ���� �Ұ�")]
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
