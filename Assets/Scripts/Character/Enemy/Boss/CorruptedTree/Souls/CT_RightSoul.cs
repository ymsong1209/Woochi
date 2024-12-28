using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_RightSoul : BaseEnemy
{
    [SerializeField] private CT_RightDummy dummySoulPrefab;
    [SerializeField] private CT_RightDummy dummySoul;
    [SerializeField] private StunResistBuff stunResistBuff;
    [SerializeField] private MoveResistBuff moveResistBuff;
    public override void Initialize()
    {
        base.Initialize();
        GameObject instantiatedEvasionBuff = Instantiate(stunResistBuff.gameObject, transform);
        StunResistBuff buff = instantiatedEvasionBuff.GetComponent<StunResistBuff>();
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
        //50% 확률로 체력이 가장 낮은 아군을 대상으로 스킬 사용
        //50% 확률로 모든 아군 캐릭터 중 하나를 무작위로 대상으로 스킬 사용
        BaseCharacter ally = null;
        if (Random.Range(0, 2) == 0)
        {
            ally = BattleUtils.FindAllyWithLeastHP(0, 1, 2, 3);
        }
        else
        {
            ally = BattleUtils.FindRandomAlly(0,1,2,3);
        }
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
            tree.RightSoul = this;
        }
        Formation enemyformation = BattleManager.GetInstance.Enemies;
        dummySoul = Instantiate(dummySoulPrefab, enemyformation.gameObject.transform).GetComponent<CT_RightDummy>();
        dummySoul.gameObject.SetActive(false);

    }

    public override void SetDead()
    {
        base.SetDead();
        dummySoul.transform.position = this.gameObject.transform.position;
        dummySoul.RowOrder = RowOrder;
        dummySoul.gameObject.SetActive(true);
        BattleManager.GetInstance.Enemies.formation[3] = dummySoul;
    }
}
