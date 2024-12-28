using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Sorcery")]
public class SorceryReward : Reward
{
    // 어떤 도술을 뽑았는지 팝업에 보여주려면 resultTxt 수정
    // 도술을 뽑지 못했을 때 팝업에 보여주려면 errorTxt 수정
    // 기본적으로 ScriotalbeObject에 있는 텍스트가 보여짐
    
    public override bool ApplyReward()
    {
        // 도술 가챠 시도 후 도술을 성공적으로 뽑았으면 도술 추가 후 true 반환
        // 도술을 뽑지 못했으면 false 반환
        int skillID = GameManager.GetInstance.Library.GetRandomSkillIDByRank();

        SkillSetResult result = GameManager.GetInstance.Library.SetSkillOnScroll(skillID);
        bool isSuccess = true;
        
        if (result.isSuccess)
        {
            if (result.isEnhanced)
            {
                resultTxt = $"{result.skillName}이 {result.enhancedSkillName}으로 강화되었습니다";
            }
            else
            {
                resultTxt = $"{result.skillName}을 획득했습니다";
            }
        }
        else if (result.isScrollFull)
        {
            errorTxt = "스크롤이 가득 찼습니다";
            isSuccess = false;
        }
        else if (result.isSameSkill)
        {
            resultTxt = $"{result.skillName}을 획득하였습니다.\n" +
                        $"그러나, {result.skillName}은 이미 강화되었습니다";
        }

        if (isSuccess)
        {
            GameManager.GetInstance.soundManager.PlaySFX("Dosul_get");
        }
        
        return isSuccess;
    }
}
