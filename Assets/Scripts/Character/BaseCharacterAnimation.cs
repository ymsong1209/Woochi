using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BaseCharacterAnimation : MonoBehaviour
{
    private BaseCharacter owner;
    private Animator animator;
    [SerializeField] private GameObject body;

    private void Start()
    {
        owner = GetComponent<BaseCharacter>();
        animator = GetComponent<Animator>();

        owner.onPlayAnimation += Play;
    }

    public void Play(AnimationType _type)
    {
        if(owner.IsDead)
        {
            return;
        }

        animator.SetTrigger(_type.ToString());
        StartCoroutine(WaitAnim());
    }

    IEnumerator WaitAnim()
    {
        owner.IsIdle = false;
        FocusIn();
        yield return null;

        // 1f�� �ϴϱ� �ִϸ��̼��� ������ �ʴ� ��찡 �־ 0.99f�� ����
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.99f)
        {
            yield return null;
        }

        owner.IsIdle = true;
        FocusOut();
    }

    void FocusIn()
    {
        body.layer = LayerMask.NameToLayer("Focus");
        // BattleManager.GetInstance.OnFocusEnter?.Invoke(owner);
    }

    void FocusOut()
    {
        body.layer = LayerMask.NameToLayer("Default");
    }

}
