using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimation : BaseCharacterAnimation
{
    [SerializeField] private List<Sprite> elementSprites;
    [SerializeField] private SpriteRenderer elementEffect;

    public override void Play(AnimationType _type)
    {
        if(owner.IsDead || !owner.IsIdle)
        {
            return;
        }

        if(_type == AnimationType.Skill0)
        {
            animator.SetInteger("SorceryIndex", Random.Range(0, 3));
            animator.SetTrigger("Sorcery");
        }
        else
        {
            animator.Play(_type.ToString());
        }

        StartCoroutine(WaitAnim());
    }

    public void ShowElement(SkillElement skillElement)
    {
        elementEffect.sprite = elementSprites[(int)skillElement];
        Invoke("HideEffect", 1.5f);
    }

    private void HideEffect()
    {
        elementEffect.sprite = null;
    }
}
