using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class BaseCharacterHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BaseCharacter owner;
    [SerializeField] private SpriteRenderer hpBar;
    [SerializeField] private TextMeshPro damageTxt;

    private void Start()
    {
        owner = GetComponent<BaseCharacter>();
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
                damageTxt.text = "빗나감";
                break;
            case AttackResult.Evasion:
                damageTxt.text = "회피";
                Invoke("DisableHUD", 2f);
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

    #region 마우스 이벤트
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (owner.IsAlly)
            return;

        UIManager.GetInstance.SetEnemyToolTip(owner);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.enemyTooltip.SetActive(false);
    }
    #endregion
}
