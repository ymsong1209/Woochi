using DG.Tweening;
using UnityEngine;

/// <summary>
/// 전투 연출 시 캐릭터가 이동할 때 사용한다
/// 피격인지, 근거리 공격인지, 원거리 공격인지에 따라 다르게 이동하는 것 구현
/// </summary>
public class Movement : StateMachineBehaviour
{
    private BaseCharacter owner;
    private float currentX = 0.0f;
    private float moveTime = 1.5f;

    [SerializeField] private bool isDamaged = false;
    [SerializeField] private bool isRemote = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(owner == null)
            owner = animator.GetComponent<BaseCharacter>();

        float moveX = 1.0f;
        currentX = owner.transform.position.x;

        // 적군이라면 이동 방향 반대로
        if (!owner.IsAlly)
            moveX *= -1f;

        // 피격일 경우 뒤로 이동, 원거리 공격일 경우 뒤로 이동
        // 피격도 아니고 근거리 공격일 경우 앞으로 이동
        if (isDamaged || isRemote)
            moveX *= -1f;
        
        animator.transform.DOMoveX(currentX + moveX, moveTime);
    }
}
