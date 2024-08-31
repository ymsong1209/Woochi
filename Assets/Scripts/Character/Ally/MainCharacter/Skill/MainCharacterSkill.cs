using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSkill : BaseSkill
{
    [SerializeField] protected int requiredSorceryPoints;
    [SerializeField] protected int rarity;
    

    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);

        // 도술 감소 처리
        MainCharacter woochi = SkillOwner as MainCharacter;
        if(woochi == null)
        {
            Debug.LogError("우치가 아님");
            return;
        }

        woochi.SorceryPoints -= RequiredSorceryPoints;
        Mathf.Clamp(woochi.SorceryPoints, 0, woochi.MaxSorceryPoints);
        UIManager.GetInstance.sorceryGuageUI.SetUI();

        var animation = SkillOwner.anim as MainCharacterAnimation;
        animation.ShowElement(SkillSO.SkillElement);
    }

    public int RequiredSorceryPoints => requiredSorceryPoints;
    public int Rarity => rarity;

}
