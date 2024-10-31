using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCharacterSkill : BaseSkill
{
    protected int requiredSorceryPoints;
    protected int rarity;
    public override void Initialize(BaseCharacter owner)
    {
        base.Initialize(owner);
        MainCharacterSkillSO mainCharacterSkillSO = SkillSO as MainCharacterSkillSO;
        requiredSorceryPoints = mainCharacterSkillSO.RequiredSorceryPoints;
        rarity = mainCharacterSkillSO.Rarity;
    }

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

    public virtual void SetSkillScrollDescription(TextMeshProUGUI skillDescription)
    {
    }

    public int RequiredSorceryPoints => requiredSorceryPoints;
    public int Rarity => rarity;

}
