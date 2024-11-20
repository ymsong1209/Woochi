using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Skill")]
public class SkillReward : Reward
{
    public override bool ApplyReward()
    {
        DataCloud.playerData.realization += 1;
        
        resultTxt = "깨달음을 획득했다\n현재 깨달음 : " + DataCloud.playerData.realization;
        return true;
    }
}
