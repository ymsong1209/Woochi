using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterHUD : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hpBar;

    private void Start()
    {
        BaseCharacter owner = GetComponent<BaseCharacter>();
        owner.Health.OnHealthChanged += UpdateHPBar;
    }

    public void UpdateHPBar(BaseCharacter _character)
    {
        float curHealth = _character.Health.CurHealth;
        float maxHealth = _character.Health.MaxHealth;
        float nextScale = curHealth / maxHealth;
        hpBar.transform.DOScaleX(nextScale, 1f).SetEase(Ease.OutCubic);
    }

}
