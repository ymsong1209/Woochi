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
        
        return true;
    }
}
