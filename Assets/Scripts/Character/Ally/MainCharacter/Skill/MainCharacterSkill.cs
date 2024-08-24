using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSkill : BaseSkill
{
    [SerializeField] protected int requiredSorceryPoints;
    
    public int RequiredSorceryPoints
    {
        get
        {
            return requiredSorceryPoints;
        }
    }

    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);

        // ���� ���� ó��
        MainCharacter woochi = SkillOwner as MainCharacter;
        if(woochi == null)
        {
            Debug.LogError("��ġ�� �ƴ�");
            return;
        }

        woochi.SorceryPoints -= RequiredSorceryPoints;
        Mathf.Clamp(woochi.SorceryPoints, 0, woochi.MaxSorceryPoints);
        UIManager.GetInstance.sorceryGuageUI.SetUI();

        var animation = SkillOwner.anim as MainCharacterAnimation;
        animation.ShowElement(SkillSO.SkillElement);
    }
}
