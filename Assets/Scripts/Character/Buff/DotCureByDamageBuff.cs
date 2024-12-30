using TMPro;
using UnityEngine;

/// <summary>
/// maxDamage의 dotCureAmount%만큼 치유함.
/// </summary>
public class DotCureByDamageBuff : BaseBuff
{
    [SerializeField] private float dotCureAmount;
    [SerializeField] private float minDamage;
    [SerializeField] private float maxDamage;
    [SerializeField] private float baseDotCureAmount;
    
    public DotCureByDamageBuff()
    {
        buffEffect = BuffEffect.DotCureByDamage;
        buffType = BuffType.Positive;
    }
    
    public override int ApplyTurnStartBuff()
    {
        //mindam~maxdam사이 dotCureAmount%만큼 힐
        int minheal = (int)(baseDotCureAmount + Mathf.Round(minDamage * dotCureAmount / 100f));
        int maxheal = (int)(baseDotCureAmount + Mathf.Round(maxDamage * dotCureAmount / 100f));
        int healAmount = Random.Range(minheal, maxheal);
        buffOwner.Health.Heal(healAmount,false);

        --buffDurationTurns;
        
        return (int)Mathf.Round(healAmount);
    }
    
    public override void AddBuff(BaseCharacter caster, BaseCharacter _buffOwner)
    {
        base.AddBuff(caster, _buffOwner);

        Stat stat = caster.FinalStat;
        minDamage = stat.GetValue(StatType.MinDamage);
        maxDamage = stat.GetValue(StatType.MaxDamage);
    }
    
    public override void StackBuff(BaseBuff _buff)
    {
        DotCureByDamageBuff damageBuff = _buff as DotCureByDamageBuff;
        if (!damageBuff) return;
        //같은 버프가 아니면 리턴
        if (damageBuff.BuffName != BuffName) return;
        // 버프 지속시간은 갱신
        base.buffDurationTurns = damageBuff.BuffDurationTurns;
        // 힐량은 덮어씌움.
        baseDotCureAmount = damageBuff.BaseDotCureAmount;
        dotCureAmount = damageBuff.DotCureAmount;
        minDamage = damageBuff.MinDamage;
        maxDamage = damageBuff.MaxDamage;
    }

    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "";
        int minheal = (int)(baseDotCureAmount + Mathf.Round(minDamage * dotCureAmount / 100f));
        int maxheal = (int)(baseDotCureAmount + Mathf.Round(maxDamage * dotCureAmount / 100f));
        description = BuffName + " " + buffDurationTurns + "턴: 매턴이 시작할 때마다 "+ minheal + " ~ " + maxheal+"만큼 회복.";
        description += "\n";
        text.text += description;
        SetBuffColor(text);
    }
    
    #region Getter Setter
    
    public float DotCureAmount
    {
        get => dotCureAmount;
        set => dotCureAmount = value;
    }

    public float MinDamage
    {
        get => minDamage;
        set => minDamage = value;
    }

    public float MaxDamage
    {
        get => maxDamage;
        set => maxDamage = value;
    }
    
    public float BaseDotCureAmount
    {
        get => baseDotCureAmount;
        set => baseDotCureAmount = value;
    }

    #endregion
}
