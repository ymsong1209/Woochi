using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterHUD : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hpBar;

    public void UpdateHPBar(float _curHealth, float _maxHealth)
    {
        float nextScale = _curHealth / _maxHealth;
        hpBar.transform.DOScaleX(nextScale, 1f).SetEase(Ease.OutCubic);
    }

}
