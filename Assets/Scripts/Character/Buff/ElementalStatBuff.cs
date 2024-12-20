using TMPro;
using UnityEngine;

public class ElementalStatBuff : BaseBuff
{
    [SerializeField] private SkillElement element;
    [SerializeField] private float changeStat;
    public ElementalStatBuff()
    {
        buffEffect = BuffEffect.ElementalStatStrengthen;
        buffType = BuffType.Positive;
        element = SkillElement.Defualt;
    }
    
    public override void StackBuff(BaseBuff inputBuff)
    {
        ElementalStatBuff elementalStatBuff = inputBuff as ElementalStatBuff;
        if (!elementalStatBuff || elementalStatBuff.BuffName!= this.BuffName || elementalStatBuff.element!= this.element) return;
        Logger.BattleLog($"\"{buffOwner.Name}({buffOwner.RowOrder + 1})\"에게 \"{buffName}\" 버프가 중첩되었습니다.", "버프 중첩");
        //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        if(inputBuff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        //아닐 경우 버프 지속시간은 갱신
        else base.buffDurationTurns = inputBuff.BuffDurationTurns;
        base.buffBattleDurationTurns += inputBuff.BuffBattleDurationTurns;
        changeStat = elementalStatBuff.ChangeStat;
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

        description += " " + changeStat + "만큼 증가\n";
        text.text += description;
        SetBuffColor(text);
    }


    public SkillElement Element => element;
    public float ChangeStat
    {
        get => changeStat;
        set => changeStat = value;
    }
}
