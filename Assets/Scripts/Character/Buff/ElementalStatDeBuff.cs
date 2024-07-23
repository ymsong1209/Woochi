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
    }
    
    public override void StackBuff(BaseBuff inputBuff)
    {
        ElementalStatDeBuff elementalStatDeBuff = inputBuff as ElementalStatDeBuff;
        if (!elementalStatDeBuff || elementalStatDeBuff.BuffName!= this.BuffName || elementalStatDeBuff.element!= this.element) return;
        //중첩시키려는 버프의 지속시간이 무한인경우 기존 버프 지속시간 무한으로 변경
        if(inputBuff.BuffDurationTurns == -1) base.buffDurationTurns = -1;
        //아닐 경우 버프 지속시간은 갱신
        else base.buffDurationTurns = inputBuff.BuffDurationTurns;
        changeStat = elementalStatDeBuff.ChangeStat;
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
        text.color = Color.red;
    }


    public SkillElement Element => element;
    public float ChangeStat
    {
        get => changeStat;
        set => changeStat = value;
    }
}
