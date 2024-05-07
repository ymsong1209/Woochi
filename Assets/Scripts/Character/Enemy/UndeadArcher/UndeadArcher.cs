using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadArcher : BaseCharacter
{
    public override void TriggerAI()
    {
        int formationIdx = BattleManager.GetInstance.Enemies.FindCharacter(this);
        if (formationIdx == 0)
        {
            ActivateRetreatShot();
        }
        else
        {
            ActivateUnholyArrow();
        }
        
        
       

      
    }

    private void ActivateUnholyArrow()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); // 0���� 99������ ���� �������� ����
        
        BaseCharacter ally = null;
        
        if (randomValue < 35) // 60% Ȯ���� 2,3,4�� �� ���� ���� ü���� �Ʊ� ����
        {
            ally = FindAllyWithLeastHP();
        }
        else // 40% Ȯ���� �����ϰ� ����
        {
            ally = FindRandomAlly();
        }
      
        if (ally != null)
        {
            BattleManager.GetInstance.SkillSelected(activeSkills[0]);
            BattleManager.GetInstance.ExecuteSelectedSkill(ally);
        }
    }
    
    public BaseCharacter FindAllyWithLeastHP()
    {
        Formation allies = BattleManager.GetInstance.Allies;
        BaseCharacter characterWithLeastHP = null;
        int lowestHP = int.MaxValue;

        //�Ʊ� 2,3,4�� �� �ϳ��� ��.
        //�Ʊ� 1,2���� formation[0],formation[1]��.
        for (int i = 1; i < 4; ++i)
        {
            BaseCharacter ally = allies.formation[i];
            if (ally != null)
            {
                int currentHP = ally.Health.CurHealth;
                if (currentHP < lowestHP)
                {
                    lowestHP = currentHP;
                    characterWithLeastHP = ally;
                }
            }
        }

        return characterWithLeastHP;
    }
    
    public BaseCharacter FindRandomAlly()
    {
        Formation allies = BattleManager.GetInstance.Allies;
        List<BaseCharacter> frontRowAllies = new List<BaseCharacter>();

        // �Ʊ� 2,3,4���� ����Ʈ�� �߰�
        for (int i = 1; i < 4; ++i)
        {
            BaseCharacter ally = allies.formation[i];
            if (ally != null)
            {
                frontRowAllies.Add(ally);
            }
        }

        if (frontRowAllies.Count > 0)
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(0, frontRowAllies.Count);
            return frontRowAllies[randomIndex];
        }

        return null; // 2,3,4���� �Ʊ��� ���� ���
    }

    private void ActivateRetreatShot()
    {
        throw new System.NotImplementedException();
    }
}
