using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Totem")]
public class TotemReward : Reward
{
    [SerializeField] private GameObject buffPrefab;

    public override bool ApplyReward()
    {
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();

        if (woochi)
        {
            var buffObject = Instantiate(buffPrefab, woochi.transform);
            var applyBuff = buffObject.GetComponent<BaseBuff>();
            woochi.ApplyBuff(woochi, woochi, applyBuff);
        }

        return true;
    }
}
