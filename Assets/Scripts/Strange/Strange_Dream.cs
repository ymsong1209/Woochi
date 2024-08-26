using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Strange_Dream : BaseStrange
{

    [SerializeField] private Button continueBtn;
    
    private void Start()
    {
        continueBtn.onClick.AddListener(() => Deactivate());
    }
        
    public override void Activate(MapNode node)
    {
        base.Activate(node);
        foreach(BaseCharacter character in BattleManager.GetInstance.Allies.formation)
        {
            if (character.IsMainCharacter)
            {
                GameObject statGameObject = new GameObject("DreamStatBuff");
                statGameObject.transform.SetParent(character.transform);
                StatBuff buff = statGameObject.AddComponent<StatBuff>();
                buff.BuffName = "길몽";
                buff.BuffDurationTurns = -1;
                buff.BuffBattleDurationTurns = 3;
                buff.changeStat.speed = 10;
                buff.IsRemovableDuringBattle = false;
                buff.IsRemoveWhenBattleEnd = false;
                character.ApplyBuff(character, character, buff);
            }
        }
    }
}
