using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoisonBuff : BaseBuff
{
    [SerializeField] private int tempPoisonStack = 0;
    [SerializeField] private int poisonStack;
    
    public override int ApplyPostHitBuff(BaseSkill skill)
    {
        int damage = 0;
        if (!isBuffAppliedThisTurn && skill.SkillSO.SkillType == SkillType.Attack && skill.SkillResult.IsHit(buffOwner))
        {
            Logger.BattleLog($"\"{buffOwner.Name}\"({buffOwner.RowOrder + 1}열)에 중독 터짐, 중독 피해 : {poisonStack}", "중독버프");
            damage = poisonStack;
            buffOwner.Health.ApplyDamage(damage);
            poisonStack = 0;
            if (tempPoisonStack == 0)
            {
                buffDurationTurns = 0;
            }
            //버프 제거를 removebuff로 호출하면 TriggerBuffs에서 index오류가 난다.
            //buffOwner.RemoveBuff(this);
        }

        if (isBuffAppliedThisTurn)
        {
            isBuffAppliedThisTurn = false;
        }
        if (tempPoisonStack > 0)
        {
            poisonStack += tempPoisonStack;
            tempPoisonStack = 0;
        }
        return damage;
    }
    
    public override int ApplyTurnEndBuff()
    {
        if (tempPoisonStack > 0)
        {
            poisonStack += tempPoisonStack;
            tempPoisonStack = 0;
        }
        return 0;
    }
    
    public override void StackBuff(BaseBuff _buff)
    {
        PoisonBuff buff = _buff as PoisonBuff;
        base.buffDurationTurns = -1;
        if (buff)
        {
            tempPoisonStack += buff.poisonStack;
            Logger.BattleLog($"\"{buffOwner.Name}\"({buffOwner.RowOrder + 1}열)에 중독 중첩, 중독 피해 : {tempPoisonStack}",
                "중독버프");
        }
    }
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        
        string description = "중독 : 피격시"+ poisonStack + "만큼 추가 피해를 입습니다.";
        text.text = description;
        SetBuffColor(text);
    }
    
    public PoisonBuff()
    {
        buffEffect = BuffEffect.Poison;
        buffType = BuffType.Negative;
        buffDurationTurns = -1;
    }
    
    public int PoisonStack
    {
        get => poisonStack;
        set => poisonStack = value;
    }
}