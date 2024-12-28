using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CT_LeftSoul : BaseEnemy
{
    [SerializeField] private CT_LeftDummy dummySoulPrefab;
    [SerializeField] private CT_LeftDummy dummySoul;
    [SerializeField] private StunResistBuff stunResistBuff;
    [SerializeField] private MoveResistBuff moveResistBuff;
    public override void Initialize()
    {
        base.Initialize();
        GameObject instantiatedStunBuff = Instantiate(stunResistBuff.gameObject, transform);
        StunResistBuff buff = instantiatedStunBuff.GetComponent<StunResistBuff>();
        buff.IsRemovableDuringBattle = false;
        buff.IsAlwaysApplyBuff = true;
        buff.BuffDurationTurns = -1;
        ApplyBuff(this,this,buff);
        
        GameObject instantiatedMoveBuff = Instantiate(moveResistBuff.gameObject, transform);
        MoveResistBuff movebuff = instantiatedMoveBuff.GetComponent<MoveResistBuff>();
        movebuff.IsRemovableDuringBattle = false;
        movebuff.IsAlwaysApplyBuff = true;
        movebuff.BuffDurationTurns = -1;
        ApplyBuff(this,this,movebuff);
    }
    public override void TriggerAI()
    {
        BaseCharacter ally = null;
        ally = BattleUtils.FindAllyFromIndex(0);
        BattleManager.GetInstance.SkillSelected(activeSkills[0]);
        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }


    // Start is called before the first frame update
    void Start()
    {
        CorruptedTree tree = BattleManager.GetInstance.Enemies.formation[1] as CorruptedTree;
        if (tree)
        {
            tree.LeftSoul = this;
        }
       
        Formation enemyformation = BattleManager.GetInstance.Enemies;
        dummySoul = Instantiate(dummySoulPrefab, enemyformation.gameObject.transform).GetComponent<CT_LeftDummy>();
        dummySoul.gameObject.SetActive(false);

    }

    public override void SetDead()
    {
        base.SetDead();
        dummySoul.transform.position = this.gameObject.transform.position;
        dummySoul.RowOrder = RowOrder;
        dummySoul.gameObject.SetActive(true);
        BattleManager.GetInstance.Enemies.formation[0] = dummySoul;
    }
}
