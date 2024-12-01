using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_RightSoul : BaseEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        CorruptedTree tree = BattleManager.GetInstance.Enemies.formation[1] as CorruptedTree;
        tree.RightSoul = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
