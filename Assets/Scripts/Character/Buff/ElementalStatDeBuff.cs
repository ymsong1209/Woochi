using TMPro;
using UnityEngine;

public class ElementalStatDeBuff : BaseBuff
{
    [SerializeField] private SkillElement element;
    [SerializeField] private float changeStat;
    
    public ElementalStatDeBuff()
    {
        buffEffect = BuffEffect.ElementalStatWeaken;
        buffType = BuffType.Negative;
        element = SkillElement.Defualt;
        buffStackType = BuffStackType.ResetDuration;
    }
    
    public override void StackBuff(BaseBuff inputBuff)
    {
        ElementalStatDeBuff elementalStatDeBuff = inputBuff as ElementalStatDeBuff;
        if (!elementalStatDeBuff || elementalStatDeBuff.BuffName!= this.BuffName || elementalStatDeBuff.element!= this.element) return;
        base.StackBuff(inputBuff);
        Logger.BattleLog($"\"{buffOwner.Name}({buffOwner.RowOrder + 1})\"에게 \"{buffName}\" 버프가 중첩되었습니다.", "버프 중첩");
    }
    
    protected override void StackBuffEffect(BaseBuff _buff)
    {
        ElementalStatDeBuff elementalStatDeBuff = _buff as ElementalStatDeBuff;
        changeStat += elementalStatDeBuff.changeStat;
    }
    
    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        string description = "";

        if (BuffDurationTurns == -1)
        {
            description = buffName + ": ";
        }
        else
        {
            description = buffName + ": " + BuffDurationTurns + "턴 동안 ";
        }

        switch (element)
        {
            case SkillElement.Fire:
                description += "화 속성 피해";
                break;
            case SkillElement.Water:
                description += "물 속성 피해";
                break;
            case SkillElement.Wood:
                description += "목 속성 피해";
                break;
            case SkillElement.Metal:
                description += "금 속성 피해";
                break;
            case SkillElement.Earth:
                description += "땅 속성 피해";
                break;
            default:
                description += "기본 피해";
                break;
        }

        description += " " + changeStat + "만큼 감소\n";
        text.text += description;
        SetBuffColor(text);
    }


    public SkillElement Element
    {
        get => element;
        set => element = value;
    }

    public float ChangeStat
    {
        get => changeStat;
        set => changeStat = value;
    }
}
