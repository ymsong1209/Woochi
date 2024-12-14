using UnityEngine;
using DG.Tweening;

public class HPBar : MonoBehaviour, ITooltipiable
{
    [SerializeField] private SpriteRenderer guage;
    private BaseCharacter owner;
    
    public void SetOwner(BaseCharacter owner)
    {
        this.owner = owner;
        owner.onHealthChanged += UpdateHPBar;
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        float maxHealth = owner.Health.MaxHealth;
        float curHealth = owner.Health.CurHealth;

        float nextScale = curHealth / maxHealth;
        guage.transform.DOScaleX(nextScale, 1f).SetEase(Ease.OutCubic);
    }

    public void ShowTooltip()
    {
        UIManager.GetInstance.SetCharacterToolTip(owner);
    }

    public void HideTooltip()
    {
        UIManager.GetInstance.characterTooltip.SetActive(false);
    }
}
