using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BaseCharacterAnimation : MonoBehaviour
{
    private BaseCharacter owner;
    private Animator animator;
    [SerializeField] private SpriteRenderer body;

    private void Awake()
    {
        owner = GetComponent<BaseCharacter>();
        animator = GetComponent<Animator>();

        owner.onPlayAnimation += Play;
        owner.onAnyTurnEnd += SetSortLayer;
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

    /// <summary>
    /// rowOrder ������ �Ŀ��� �ִ� ĳ���Ͱ� �տ� ���̵���
    /// </summary>
    void SetSortLayer()
    {
        body.sortingOrder = owner.rowOrder;
    }    

    void FocusIn()
    {
        body.gameObject.layer = LayerMask.NameToLayer("Focus");
        // BattleManager.GetInstance.OnFocusEnter?.Invoke(owner);
    }

    void FocusOut()
    {
        body.gameObject.layer = LayerMask.NameToLayer("Default");
    }

}
