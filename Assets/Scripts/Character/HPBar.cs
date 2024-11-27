using UnityEngine;
using DG.Tweening;

public class HPBar : MonoBehaviour, ITooltipiable
{
    [SerializeField] private SpriteRenderer guage;
    private SpriteRenderer bar;
    private float maxHealth;
    private float curHealth;

    void Start()
    {
        bar = GetComponent<SpriteRenderer>();
    }

    public void SetHPBar(Health health)
    {
        maxHealth = health.MaxHealth;
        curHealth = health.CurHealth;

        float nextScale = curHealth / maxHealth;
        guage.transform.DOScaleX(nextScale, 1f).SetEase(Ease.OutCubic);
    }

    public string GetTooltipText()
    {
        return $"{curHealth} / {maxHealth}";
    }

    public Vector3 GetPosition()
    {
        float height = bar.bounds.size.y;
        Vector3 worldPos = bar.bounds.center - height * Vector3.up;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        return screenPos;
    }

    public void ShowTooltip()
    {
        UIManager.GetInstance.SetHPTooltip(true, this);
    }

    public void HideTooltip()
    {
        UIManager.GetInstance.SetHPTooltip(false);
    }
}
