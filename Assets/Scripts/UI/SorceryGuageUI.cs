using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SorceryGuageUI : MonoBehaviour
{
    [HideInInspector] public MainCharacter woochi;

    [SerializeField] private Image sorceryGuage;
    [SerializeField] private Image sorceryGuageBackground;
    [SerializeField] private TextMeshProUGUI sorceryGuageText;

    public void SetUI()
    {
        sorceryGuage.fillAmount = GetScale(woochi.SorceryPoints);
        sorceryGuageBackground.fillAmount = GetScale(woochi.SorceryPoints);
        sorceryGuageText.text = $"{woochi.SorceryPoints}";
    }

    public void ShowAnimation(int changeGuage = 0, bool isMinus = false)
    {
        if(isMinus) changeGuage *= -1;

        sorceryGuage.DOFillAmount(GetScale(woochi.SorceryPoints + changeGuage), 1f).SetEase(Ease.OutCubic);
        sorceryGuageBackground.DOFillAmount(GetScale(woochi.SorceryPoints), 1f).SetEase(Ease.OutCubic);
        sorceryGuageText.text = $"{woochi.SorceryPoints + changeGuage}";
    }

    public void Restore()
    {
        sorceryGuage.DOFillAmount(GetScale(woochi.SorceryPoints), 1f).SetEase(Ease.OutCubic);
        sorceryGuageBackground.DOFillAmount(GetScale(woochi.SorceryPoints), 1f).SetEase(Ease.OutCubic);
        sorceryGuageText.text = $"{woochi.SorceryPoints}";
    }

    private float GetScale(int point)
    {
        point = Mathf.Clamp(point, 0, woochi.MaxSorceryPoints);
        return (float)point / woochi.MaxSorceryPoints;
    }
}
