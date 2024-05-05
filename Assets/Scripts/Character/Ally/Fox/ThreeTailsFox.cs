using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeTailsFox : BaseCharacter
{
    public override void CheckSkillsOnTurnStart()
    {
        activeSkills.Clear();
        int characterIndex = BattleManager.GetInstance.GetCharacterIndex(this);
        
        #region 꼬리불 처리
        if (characterIndex == 0 || characterIndex == 1)
        {
            activeSkills.Add(totalSkills[1]);
        }
        else if(characterIndex == 2 || characterIndex == 3)
        {
            activeSkills.Add(totalSkills[0]);
        }
        #endregion

        #region 여우불
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
            //3열과 4열중 어디에도 없을 경우 여우불 없음
        }
        #endregion

        #region 홀리기
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
            //3열과 4열중 어디에도 없을 경우 홀리기 없음
            //activeSkills.Add(totalSkills[4]);
        }
        #endregion
    }
}