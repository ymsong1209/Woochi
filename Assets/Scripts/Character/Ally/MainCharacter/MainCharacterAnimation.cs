using UnityEngine;

public class MainCharacterAnimation : BaseCharacterAnimation
{
    public override void Play(AnimationType _type)
    {
        if(owner.IsDead)
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
}
