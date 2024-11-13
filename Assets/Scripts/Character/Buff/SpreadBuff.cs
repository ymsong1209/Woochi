using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpreadBuff : BaseBuff
{
    [Header("SpreadBuff")]
    [SerializeField] private GameObject buff;
    [SerializeField] private bool forAllies;
    [SerializeField] private bool forWoochi;
    [SerializeField] private bool forEnemies;

    public override int ApplyBattleStartBuff()
    {
        MainCharacter woochi = BattleManager.GetInstance.Allies.GetWoochi();
        if (forAllies)
        {
            var allies = BattleManager.GetInstance.Allies.AllCharacter;
            foreach (var ally in allies)
            {
                var buffObject = Instantiate(buff, ally.transform);
                var applyBuff = buffObject.GetComponent<BaseBuff>();
                ally.ApplyBuff(woochi, ally, applyBuff);
            }
        }
        else if (forWoochi)
        {
            if (woochi)
            {
                var buffObject = Instantiate(buff, woochi.transform);
                var applyBuff = buffObject.GetComponent<BaseBuff>();
                woochi.ApplyBuff(woochi, woochi, applyBuff);
            }
        }
        
        if (forEnemies)
        {
            var enemies = BattleManager.GetInstance.Enemies.AllCharacter;
            foreach (var enemy in enemies)
            {
                var buffObject = Instantiate(buff, enemy.transform);
                var applyBuff = buffObject.GetComponent<BaseBuff>();
                enemy.ApplyBuff(woochi, enemy, applyBuff);
            }
        }
        
        return 0;
    }

    public override void SetBuffDescription(TextMeshProUGUI text)
    {
        text.text += $"{buffName} : {buffBattleDurationTurns}번의 전투동안 지속\n";
    }
}
