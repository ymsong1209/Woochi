using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_RightSoul : BaseEnemy
{
    [SerializeField] private CT_RightDummy dummySoul;

    public override void TriggerAI()
    {
        Debug.Log("RightSoulAI");
    }


    // Start is called before the first frame update
    void Start()
    {
        CorruptedTree tree = BattleManager.GetInstance.Enemies.formation[1] as CorruptedTree;
        tree.LeftSoul = this;
        GameObject dummy = Instantiate(dummySoul, transform.position, Quaternion.identity).gameObject;
        dummy.SetActive(false);

    }

    public override void SetDead()
    {
        base.SetDead();
        dummySoul.gameObject.SetActive(true);
        BattleManager.GetInstance.Enemies.formation[0] = dummySoul;
    }
}
