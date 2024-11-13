using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// 우치 전용 보상
/// </summary>
[CreateAssetMenu(fileName = "Reward_", menuName = "Scriptable Objects/Reward/Woochi")]
public class WoochiReward : Reward
{
    [Header("Heal")]
    [SerializeField] private bool heal;             // 체력 회복 여부
    [SerializeField] private int healAmount;        // 체력 회복 수치
    [SerializeField] private bool sorceryHeal;      // 도력 회복 여부

    [Header("Stat Up")]
    [SerializeField] private bool fixedStatUp;      // 고정 스탯 상승 여부
    [SerializeField] private StatType fixType;         // 상승시킬 스탯
    [SerializeField] private bool randomStatUp;     // 랜덤 스탯 상승 여부
    [SerializeField] private bool allStatUp;        // 모든 스탯 상승 여부
    [SerializeField] private int statUpAmount;      // 증가시킬 스탯 양

    public override bool ApplyReward()
    {
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();

        if(heal)
        {
            woochi.Health.Heal(healAmount, false);
        }

        if (sorceryHeal)
        {
            woochi.UpdateSorceryPoints(999, true);
        }
        
        if(fixedStatUp || randomStatUp || allStatUp)
        {
            StatUp(woochi);
        }

        return true;
    }

    private void StatUp(MainCharacter woochi)
    {
        Stat stat = new Stat();

        if(randomStatUp)
        {
            StatType type = (StatType)Random.Range(0, (int)StatType.END);
            if (type == StatType.MinDamage || type == StatType.MaxDamage)
            {
                stat.SetValue(StatType.MinDamage, statUpAmount);
                stat.SetValue(StatType.MaxDamage, statUpAmount);
            }
            else
            {
                stat.SetValue(type, statUpAmount);
            }
            
            resultTxt += $"우치의 {type.GetDisplayName()}이(가) {statUpAmount} 증가했습니다";
        }
        else if (allStatUp)
        {
            for (int i = 0; i < (int)StatType.END; i++)
            {
                stat.SetValue((StatType)i, statUpAmount);
            }
            
            resultTxt = $"우치의 모든 스탯이 {statUpAmount} 증가했습니다";
        }
        else if (fixedStatUp)
        {
            if(fixType == StatType.MinDamage || fixType == StatType.MaxDamage)
            {
                stat.SetValue(StatType.MinDamage, statUpAmount);
                stat.SetValue(StatType.MaxDamage, statUpAmount);
            }
            else
            {
                stat.SetValue(fixType, statUpAmount);
            }
            
            resultTxt += $"우치의 {fixType.GetDisplayName()}이(가) {statUpAmount} 증가했습니다";
        }
        woochi.rewardStat += stat;
    }

    private void OnValidate()
    {
        if(fixedStatUp)
        {
            randomStatUp = false;
            allStatUp = false;
        }
        else if (randomStatUp)
        {
            fixedStatUp = false;
            allStatUp = false;
        }
        else if (allStatUp)
        {
            fixedStatUp = false;
            randomStatUp = false;
        }
    }
}
