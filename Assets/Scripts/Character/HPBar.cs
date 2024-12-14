using UnityEngine;
using DG.Tweening;

public class HPBar : MonoBehaviour
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
}
