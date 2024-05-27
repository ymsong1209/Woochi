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

        // 1f로 하니까 애니메이션이 끝나지 않는 경우가 있어서 0.99f로 수정
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.99f)
        {
            yield return null;
        }

        owner.IsIdle = true;
        FocusOut();
    }

    /// <summary>
    /// rowOrder 값으로 후열에 있는 캐릭터가 앞에 보이도록
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
