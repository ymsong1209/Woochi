using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDog_Bite : BaseSkill
{
    [SerializeField] private GameObject bleedPrefab;
    public override void ActivateSkill(BaseCharacter _opponent)
    {
        
        //아군 보호 스킬등으로 보호 할 수 있음
        //최종적으로 공격해야하는 적 판정
        BaseCharacter opponent = _opponent;
        if (_opponent.IsAlly)
        {
            opponent = CheckOpponentValid(_opponent);
        }
      
        if(opponent == null)
        {
            Debug.LogError("opponent is null");
            return;
        }
      
        GameObject bleedDebuffPrefab = Bufflist[0];
        GameObject bleedDebuffGameObject = Instantiate(bleedDebuffPrefab, transform);
        BleedDeBuff bleedDebuff = bleedDebuffGameObject.GetComponent<BleedDeBuff>();
        bleedDebuff.BuffDurationTurns = 3;
        bleedDebuff.ChanceToApplyBuff = 65;
        bleedDebuff.BleedPercent = 3;
        instantiatedBuffList.Add(bleedDebuffGameObject);
        
        base.ActivateSkill(opponent);
    }
}
