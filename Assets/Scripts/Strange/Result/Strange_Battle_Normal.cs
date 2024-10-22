using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/Battle/Normal")]
public class Strange_Battle_Normal : StrangeResult
{
    public override void ApplyEffect()
    {
        StrangeManager.GetInstance.isBattleStrange = true;

        var IDs = MapManager.GetInstance.config.GetNormalEnemy();
        BattleManager.GetInstance.InitializeBattle(IDs);
    }
}
