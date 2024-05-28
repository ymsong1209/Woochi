using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimation : BaseCharacterAnimation
{
    [SerializeField] Sprite[] magicSprites;
    [SerializeField] Sprite[] magicEffects;

    public override void Play(AnimationType _type)
    {
        base.Play(_type);

        if(_type == AnimationType.Skill0)
        {
            
        }
    }
}
