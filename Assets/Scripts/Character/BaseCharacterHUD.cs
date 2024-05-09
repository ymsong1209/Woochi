using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseCharacterHUD : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hpBar;
    [SerializeField] private TextMeshPro damageTxt;

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

    public void UpdateDamage(AttackResult _result, int damage = 0, bool isCrit = false)
    {
        damageTxt.gameObject.SetActive(true);
        damageTxt.color = Color.white;
        damageTxt.fontSize = 10;

        switch (_result)
        {
            case AttackResult.Miss:
                damageTxt.text = "ºø³ª°¨";
                break;
            case AttackResult.Evasion:
                damageTxt.text = "È¸ÇÇ";
                break;
            case AttackResult.Normal:
                damageTxt.text = $"{damage}";
                if(isCrit)
                {
                    damageTxt.color = Color.red;
                    damageTxt.fontSize = 15;
                }
                break;
        }
    }

    public void DisableHUD()
    {
        damageTxt.gameObject.SetActive(false);
    }
}
