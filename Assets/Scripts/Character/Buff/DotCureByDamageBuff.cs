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
    
    public DotCureByDamageBuff()
    {
        buffEffect = BuffEffect.DotCureByDamage;
        buffType = BuffType.Positive;
    }
    
    public override int ApplyTurnStartBuff()
    {
        //maxDamage의 dorCureAmount%만큼 힐
        float healAmount = maxDamage * dotCureAmount / 100f;
        buffOwner.Health.Heal((int)Mathf.Round(healAmount),false);

        --buffDurationTurns;
        
        return (int)Mathf.Round(healAmount);
    }
    
    public override void AddBuff(BaseCharacter caster, BaseCharacter _buffOwner)
    {
        base.AddBuff(caster, _buffOwner);

        Stat stat = caster.Stat;
        minDamage = stat.minStat;
        maxDamage = stat.maxStat;
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
        dotCureAmount = damageBuff.DotCureAmount;
        minDamage = damageBuff.MinDamage;
        maxDamage = damageBuff.MaxDamage;
    }

    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "";
        description = BuffName + ": 매턴마다 "+  (int)Mathf.Round(maxDamage * dotCureAmount / 100f)+"만큼 회복.";
        description += "\n";
        text.text += description;
        text.color = Color.blue;
    }
    
    #region Getter Setter
    
    public float DotCureAmount
    {
        get => dotCureAmount;
        set => dotCureAmount = value;
    }
    public float MinDamage => minDamage;
    public float MaxDamage => maxDamage;

    #endregion
}
