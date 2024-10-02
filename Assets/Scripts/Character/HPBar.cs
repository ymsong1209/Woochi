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
        Vector3 center = bar.bounds.center;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(center);
        Vector3 offset = new Vector3(0, -50, 0);
        return screenPos + offset;
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
