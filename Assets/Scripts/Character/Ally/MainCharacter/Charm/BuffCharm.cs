using UnityEngine;
using TMPro;

[RequireComponent(typeof(StatBuff))]
public class BuffCharm : BaseCharm
{
    #region 변화된 스탯들의 수치
    [SerializeField] protected Stat changeStat;
    #endregion 변화된 스탯들 frame update

    public override void Activate(BaseCharacter opponent)
    {
        if(opponent == null || opponent.IsDead) return;

        BaseCharm charmObject = Instantiate(this);
        StatBuff buff = charmObject.GetComponent<StatBuff>();
        buff.BuffName = CharmName;
        buff.BuffDurationTurns = Turns;
        buff.changeStat = changeStat;
        BaseCharacter caster = BattleManager.GetInstance.Allies.GetWoochi();
        
        opponent.ApplyBuff( caster,opponent, buff);
    }

    public override void SetCharmDescription(TextMeshProUGUI text)
    {
        base.SetCharmDescription(text);
        string description = text.text;
        if (CharmTargetType == CharmTargetType.Singular)
        {
            description += Turns + "턴 동안 아군 ";
        }
        else if (CharmTargetType == CharmTargetType.SingularWithSelf)
        {
            description += Turns + "턴 동안 자신 및 아군 1명의 ";
        }
        
        int statCount = 0;

        for (int i = 1; i < (int)StatType.MaxDamage; i++)
        {
            if (!Mathf.Approximately(changeStat.GetValue((StatType)i), 0))
            {
                if (statCount > 0 && statCount % 2 == 0) description += "\n";
                description += ((StatType)i).GetDisplayName() + " : " + changeStat.GetValue((StatType)i) + " ";
                statCount++;
            }
        }

        text.text = description;
    }
}
