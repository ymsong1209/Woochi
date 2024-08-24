
using UnityEngine;

[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Exp")]
public class ExpReward : Reward
{
    [SerializeField] private int expAmount;      // ����ġ ��

    public override bool ApplyReward()
    {
        var allyList = BattleManager.GetInstance.Allies.GetAllies();

        // ���� ��ȯ�� ����
        BaseCharacter ally = allyList.Random();

        if(ally.IsDead || ally.level.IsMaxRank())
        {
            HelperUtilities.ShowRewardToolResult($"{ally.Name}��(��) ���� ����ġ�� ���� �� �����ϴ�");
            return false;
        }

        ally.level.plusExp += expAmount;
        HelperUtilities.ShowRewardToolResult($"{ally.Name}��(��) {expAmount}�� ����ġ�� ȹ���߽��ϴ�");
        return true;
    }
}
