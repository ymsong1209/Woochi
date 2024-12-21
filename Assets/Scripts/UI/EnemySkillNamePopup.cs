using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class EnemySkillNamePopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI skillNameText; // 스킬 이름 텍스트
    [SerializeField] private Image BGImage; // 배경 이미지

    private Vector3 originalScale; // 처음 크기 저장

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        BGImage.fillAmount = 0f;
    }

    public IEnumerator ShowUI(string skillName)
    {
        // 텍스트 설정
        skillNameText.text = skillName;

        // UI 활성화 (스케일 0, 알파 0 상태로 시작)
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        BGImage.fillAmount = 0f;

        // DOTween Sequence 생성
        Sequence seq = DOTween.Sequence();

        // 1) 스케일 0 -> (원래 * 1.1)로 (0.5초)
        //    동시에 알파 0 -> 1 로 (0.5초)
        seq.Append(
            transform.DOScale(originalScale * 1.05f, 0.25f)
                .SetEase(Ease.OutBack)
        );
        seq.Join(
            canvasGroup.DOFade(1f, 0.3f)
                .SetEase(Ease.InOutQuad)
        );
        seq.Join(
            BGImage.DOFillAmount(1f, 0.3f)
                .SetEase(Ease.InOutQuad)
        );

        // 2) 스케일 (원래 * 1.1) -> 원래크기 (0.25초)
        seq.Append(
            transform.DOScale(originalScale, 0.1f)
                .SetEase(Ease.InBack)
        );

        // 시퀀스 재생 완료까지 대기
        yield return seq.WaitForCompletion();
        
        yield return new WaitForSeconds(1.5f);
        
        
//         // DOTween Sequence 생성
//         Sequence fadeseq = DOTween.Sequence();
//
//         // 1) 스케일 0 -> (원래 * 1.1)로 (0.5초)
//         //    동시에 알파 0 -> 1 로 (0.5초)
//         fadeseq.Append(
//             canvasGroup.DOFade(0f, 1f)
//                 .SetEase(Ease.InOutQuad)
//             // transform.DOScale(Vector3.zero, 2f)
//             //     .SetEase(Ease.OutBack)
//         );
//         // fadeseq.Join(
//         //     canvasGroup.DOFade(0f, 1f)
//         //         .SetEase(Ease.InOutQuad)
//         // );
//
// // 트위닝이 끝날 때까지 코루틴에서 대기하려면
//         yield return fadeseq.WaitForCompletion();

        // 원하는 후처리 (여기서는 알파 내리면서 사라지거나,
        // 바로 SetActive(false)로 끄기도 가능)
        gameObject.SetActive(false);
    }
}