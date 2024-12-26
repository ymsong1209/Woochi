using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_RightSoul : BaseEnemy
{
    [SerializeField] private CT_RightDummy dummySoulPrefab;
    [SerializeField] private CT_RightDummy dummySoul;
    [SerializeField] private StunResistBuff stunResistBuff;
    [SerializeField] private MoveResistBuff moveResistBuff;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject hp;
    
    [SerializeField] private bool soulDead = false; //IsDead와 별개로 죽었는지 판단. 부활스킬 사용위해 변수 따로 만듬
    public override void Initialize()
    {
        base.Initialize();
        InitializeBuffs();
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
    
    private void InitializeBuffs()
    {
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
        BuffList.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
        ground.SetActive(false);
        hp.SetActive(false);
        soulDead = true;
    }

    public void Revive()
    {
        Resurrect(true);
        Health.CurHealth = (int)(Health.MaxHealth * 0.30f);
        ground.SetActive(true);
        hp.SetActive(true);
        soulDead = false;
        BuffList.gameObject.SetActive(true);
        InitializeBuffs();
    }
    
    public bool SoulDead=>soulDead;
    public SpriteRenderer Body => body;
}
