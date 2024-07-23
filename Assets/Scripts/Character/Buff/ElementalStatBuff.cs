using TMPro;
using UnityEngine;

public class ElementalStatBuff : BaseBuff
{
    #region 변화된 스탯들의 수치
    [SerializeField] private SkillElement element;
    #endregion 변화된 스탯들
    public ElementalStatBuff()
    {
        buffEffect = BuffEffect.ElementalStatStrengthen;
        buffType = BuffType.Positive;
    }
    
    public override void AddBuff(BaseCharacter caster, BaseCharacter _buffOwner)
    {
       base.AddBuff(caster, _buffOwner);
       buffOwner.CheckForStatChange();
    }
    
    public override void StackBuff(BaseBuff inputBuff)
    {
        // ElementalStatBuff elementalStatBuff = inputBuff as ElementalStatBuff;
        // if (!elementalStatBuff || elementalStatBuff.BuffName!= this.BuffName || elementalStatBuff.element!= this.element) return;
        // //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        // if(_buff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        // else base.buffDurationTurns += _buff.BuffDurationTurns;
        //
        // changeStat += statBuff.ChangeStat;
        // buffOwner.CheckForStatChange();
    }

    public override void RemoveBuff()
    {
        buffOwner.CheckForStatChange();
        base.RemoveBuff();
    }
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "";
        
    }


    public SkillElement Element => element;

}
