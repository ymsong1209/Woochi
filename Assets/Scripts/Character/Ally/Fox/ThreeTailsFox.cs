using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeTailsFox : BaseCharacter
{
    public override void CheckSkillsOnTurnStart()
    {
        activeSkills.Clear();
        int characterIndex = BattleManager.GetInstance.GetCharacterIndex(this);
        // 이게 하드 코딩을 안할려고 했는데, 좀 생각해보기
        // 현재 위치에서 쓸 수 있는 스킬만 넣는다고 치면 스킬이 1개만 들어갈 수도 있고 그래서임
        // 쓸 수 있는 스킬 1개만 넣어도 된다고 하면 수정
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
            activeSkills.Add(totalSkills[2]);
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
            activeSkills.Add(totalSkills[4]);
        }
        #endregion
    }
}