using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeTailsFox : BaseCharacter
{
    public override void CheckSkillsOnTurnStart()
    {
        activeSkills.Clear();
        int characterIndex = BattleManager.GetInstance.GetCharacterIndex(this);
        // �̰� �ϵ� �ڵ��� ���ҷ��� �ߴµ�, �� �����غ���
        // ���� ��ġ���� �� �� �ִ� ��ų�� �ִ´ٰ� ġ�� ��ų�� 1���� �� ���� �ְ� �׷�����
        // �� �� �ִ� ��ų 1���� �־ �ȴٰ� �ϸ� ����
        #region ������ ó��
        if (characterIndex == 0 || characterIndex == 1)
        {
            activeSkills.Add(totalSkills[1]);
        }
        else if(characterIndex == 2 || characterIndex == 3)
        {
            activeSkills.Add(totalSkills[0]);
        }
        #endregion

        #region �����
        if (characterIndex == 2)
        {
            activeSkills.Add(totalSkills[2]);
        }
        else if(characterIndex == 3)
        {
            activeSkills.Add(totalSkills[3]);
        }
        else
        {
            activeSkills.Add(totalSkills[2]);
        }
        #endregion

        #region Ȧ����
        if (characterIndex == 2)
        {
            activeSkills.Add(totalSkills[5]);
        }
        else if(characterIndex == 3)
        {
            activeSkills.Add(totalSkills[4]);
        }
        else
        {
            activeSkills.Add(totalSkills[4]);
        }
        #endregion
    }
}
