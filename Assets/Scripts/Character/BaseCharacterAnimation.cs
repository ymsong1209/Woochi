using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BaseCharacterAnimation : MonoBehaviour
{
    protected BaseCharacter owner;
    protected Animator animator;
    [SerializeField] protected  SpriteRenderer body;

    private void Awake()
    {
        owner = GetComponent<BaseCharacter>();
        animator = GetComponent<Animator>();

        owner.onPlayAnimation += Play;
    }

    public virtual void Play(AnimationType _type)
    {
        animator.Play(_type.ToString());
        StartCoroutine(WaitAnim());
    }

    protected IEnumerator WaitAnim()
    {
        owner.IsIdle = false;
        FocusIn();

        yield return new WaitForSeconds(1.5f);

        owner.IsIdle = true;
        FocusOut();

        animator.Play("Idle");
    }

    public void PlayDeadAnimation() => animator.Play("Dead");

    /// <summary>
    /// rowOrder 값으로 후열에 있는 캐릭터가 앞에 보이도록
    /// </summary>
    public void SetSortLayer(int _rowOrder)
    {
        body.sortingOrder = _rowOrder;
    }    

    void FocusIn()
    {
        body.gameObject.layer = LayerMask.NameToLayer("Focus");
        BattleManager.GetInstance.OnFocusEnter?.Invoke(owner);
    }

    void FocusOut()
    {
        body.gameObject.layer = LayerMask.NameToLayer("Default");
    }

}
