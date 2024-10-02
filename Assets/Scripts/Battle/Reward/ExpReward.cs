
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Exp")]
public class ExpReward : Reward
{
    [SerializeField] private int expAmount;      // 경험치 양

    public override bool ApplyReward()
    {
        var allyList = BattleManager.GetInstance.Allies.GetAllies();

        // 랜덤 소환수 선택
        BaseCharacter ally = allyList.Random();

        if(ally.IsDead || ally.level.IsMaxRank())
        {
            resultTxt = $"{ally.Name}은(는) 현재 경험치를 얻을 수 없습니다";
            return false;
        }

        ally.level.plusExp += expAmount;
        resultTxt = $"{ally.Name}은(는) {expAmount}의 경험치를 획득했습니다";
        return true;
    }
}
