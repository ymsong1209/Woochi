using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

[RequireComponent(typeof(StatBuff))]
public class BuffCharm : BaseCharm
{
    #region 변화된 스탯들의 수치
    [SerializeField] protected Stat changeStat;
    #endregion 변화된 스탯들 frame update

    public override void Activate(BaseCharacter opponent)
    {
        StatBuff buff = GetComponent<StatBuff>();
        buff.BuffName = CharmName;
        buff.BuffDurationTurns = Turns;
        buff.changeStat = changeStat;
        BaseCharacter caster = BattleManager.GetInstance.currentCharacter;
        if (caster.IsMainCharacter)
        {
            opponent.ApplyBuff( caster,opponent, buff);
        }
        
    }

    public override void SetCharmDescription(TextMeshProUGUI text)
    {
        base.SetCharmDescription(text);
        string description = text.text;
        int statCount = 0;
        
        if (!Mathf.Approximately(changeStat.defense, 0))
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "방어력 : " + changeStat.defense + " ";
            statCount++;
        }
        if (!Mathf.Approximately(changeStat.crit, 0))
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "치명타 : " + changeStat.crit + " ";
            statCount++;
        }
        if (!Mathf.Approximately(changeStat.accuracy, 0))
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "명중 : " + changeStat.accuracy + " ";
            statCount++;
        }
        if (!Mathf.Approximately(changeStat.evasion, 0))
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "회피 : " + changeStat.evasion + " ";
            statCount++;
        }
        if (!Mathf.Approximately(changeStat.resist, 0))
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "저항 : " + changeStat.resist + " ";
            statCount++;
        }
        if (!Mathf.Approximately(changeStat.minStat, 0))
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "최소 스탯 : " + changeStat.minStat + " ";
            statCount++;
        }
        if (!Mathf.Approximately(changeStat.maxStat, 0))
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "최대 스탯 : " + changeStat.maxStat + " ";
            statCount++;
        }
        if (!Mathf.Approximately(changeStat.speed, 0))
        {
            if (statCount > 0 && statCount % 2 == 0) description += "\n";
            description += "속도 : " + changeStat.speed + " ";
            statCount++;
        }

        text.text = description;
    }
}
