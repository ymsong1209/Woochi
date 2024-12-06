using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorruptedTree : BaseEnemy
{
    [SerializeField] private BaseBuff fearBuff;
    private BaseCharacter leftSoul;
    private BaseCharacter rightSoul;
    public override void TriggerAI()
    {
        Formation allyFormation = BattleManager.GetInstance.Allies;
       
        //두려움 버프를 안가진 아군이 하나라도 있을때
        bool canApplyFear = false;
        foreach(BaseCharacter character in allyFormation.formation)
        {
            if (character && !character.IsDead && !character.FindMatchingBuff(fearBuff))
            {
                canApplyFear = true;
                break;
            }
        }
        
        //두려움 버프를 안가진 아군이 하나라도 있을때
        if (canApplyFear)
        {
            //30% 확률로 기술1, 40% 확률로 기술2, 30% 확률로 기술3
            System.Random random = new System.Random();
            int randomValue = random.Next(0, 100);
            if (randomValue < 30)
            {
                FearWhisper();
            }
            else if (randomValue < 70)
            {
                BasicAttack();
            }
            else
            {
                ResurrectAndBuff();
            }
        }
        else
        {
            //40% 확률로 기술2, 60% 확률로 기술3
            System.Random random = new System.Random();
            int randomValue = random.Next(0, 100);
            if (randomValue < 40)
            {
                BasicAttack();
            }
            else
            {
                ResurrectAndBuff();
            }
        }
    }


    private void FearWhisper()
    {
        BaseCharacter receiver = null;
        Formation allyFormation = BattleManager.GetInstance.Allies;
        List<BaseCharacter> allywithNoFear = new List<BaseCharacter>();
        foreach(BaseCharacter character in allyFormation.formation)
        {
            if (character && !character.IsDead && !character.FindMatchingBuff(fearBuff))
            {
                allywithNoFear.Add(character);
            }
        }
        
        System.Random random = new System.Random();
        int randomValue = random.Next(0, allywithNoFear.Count);
        receiver = allywithNoFear[randomValue];
        
        BattleManager.GetInstance.SkillSelected(activeSkills[0]);
        BattleManager.GetInstance.CharacterSelected(receiver);
        BattleManager.GetInstance.ExecuteSelectedSkill(receiver);
    }

    private void BasicAttack()
    {
        BaseCharacter receiver = null;
        receiver = BattleUtils.FindRandomAlly(0, 1, 2, 3);
        BattleManager.GetInstance.SkillSelected(activeSkills[1]);
        BattleManager.GetInstance.CharacterSelected(receiver);
        BattleManager.GetInstance.ExecuteSelectedSkill(receiver);
    }

    private void ResurrectAndBuff()
    {
        BaseCharacter receiver = null;
        Formation enemyFormation = BattleManager.GetInstance.Enemies;
        List<BaseCharacter> deadSouls = new List<BaseCharacter>();
        if (enemyFormation.formation[0] && enemyFormation.formation[0].IsDead)
        {
            deadSouls.Add(enemyFormation.formation[0]);
        }
        if (enemyFormation.formation[3] && enemyFormation.formation[3].IsDead)
        {
            deadSouls.Add(enemyFormation.formation[3]);
        }
        if(deadSouls.Count > 0)
        {
            System.Random random = new System.Random();
            int randomValue = random.Next(0, deadSouls.Count);
            BaseCharacter deadSoul = deadSouls[randomValue];
            int index = enemyFormation.FindCharacterIndex(deadSoul);
            deadSoul.gameObject.SetActive(false);
            enemyFormation.formation[index] = null;
            if (index == 0)
            {
                leftSoul.gameObject.SetActive(true);
                leftSoul.Resurrect(true);
                leftSoul.Health.CurHealth = (int)(leftSoul.Health.MaxHealth * 0.44f);
                enemyFormation.formation[0] = leftSoul;
            }
            else if (index == 3)
            {
                rightSoul.gameObject.SetActive(true);
                rightSoul.Resurrect(true);
                rightSoul.Health.CurHealth = (int)(rightSoul.Health.MaxHealth * 0.44f);
                enemyFormation.formation[3] = rightSoul;
            }
        }
        
        
        BattleManager.GetInstance.SkillSelected(activeSkills[2]);
        receiver = BattleUtils.FindRandomAlly(0, 3);
        BattleManager.GetInstance.CharacterSelected(receiver);
        BattleManager.GetInstance.ExecuteSelectedSkill(receiver);
        
    }

    public BaseCharacter LeftSoul
    {
        get => leftSoul;
        set => leftSoul = value;
    }
    public BaseCharacter RightSoul
    {
        get => rightSoul;
        set => rightSoul = value;
    }
}
