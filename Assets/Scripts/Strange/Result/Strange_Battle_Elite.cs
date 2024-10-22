using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/Battle/Elite")]
public class Strange_Battle_Elite : StrangeResult
{
    public override void ApplyEffect()
    {
        StrangeManager.GetInstance.isBattleStrange = true;

        var IDs = MapManager.GetInstance.config.GetEliteEnemy();
        BattleManager.GetInstance.InitializeBattle(IDs, 100, true);
    }
}
