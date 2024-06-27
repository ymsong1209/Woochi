using DG.Tweening;
using UnityEngine;

/// <summary>
/// ���� ���� �� ĳ���Ͱ� �̵��� �� ����Ѵ�
/// �ǰ�����, �ٰŸ� ��������, ���Ÿ� ���������� ���� �ٸ��� �̵��ϴ� �� ����
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

        // �����̶�� �̵� ���� �ݴ��
        if (!owner.IsAlly)
            moveX *= -1f;

        // �ǰ��� ��� �ڷ� �̵�, ���Ÿ� ������ ��� �ڷ� �̵�
        // �ǰݵ� �ƴϰ� �ٰŸ� ������ ��� ������ �̵�
        if (isDamaged || isRemote)
            moveX *= -1f;
        
        animator.transform.DOMoveX(currentX + moveX, moveTime);
    }
}
