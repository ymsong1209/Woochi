using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class BaseCharacterHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BaseCharacter owner;
    [SerializeField] private HPBar hpBar;
    [SerializeField] private SpriteRenderer ground;
    [SerializeField] private GameObject turnEffect;

    [Header("Damage HUD")]
    [SerializeField] private GameObject damageHUD;
    [SerializeField] private TextMeshPro damageTxt;

    private void Awake()
    {
        owner = GetComponent<BaseCharacter>();
        owner.onHealthChanged += UpdateHPBar;
        owner.onAttacked += SetDamageText;
    }

    public void UpdateHPBar()
    {
        hpBar.SetHPBar(owner.Health);
    }

    public void SetDamageText(AttackResult _result, int damage = 0, bool isCrit = false)
    {
        damageHUD.SetActive(true);
        float duration = 1f;

        if (isCrit)
        {
            damageTxt.color = Color.red;
            damageTxt.fontSize = 17;
            damageTxt.fontStyle = FontStyles.Bold;
            duration = 2f;
        }
        else
        {
            damageTxt.color = Color.white;
            damageTxt.fontSize = 10;
            damageTxt.fontStyle = FontStyles.Normal;
        }

        switch (_result)
        {
            case AttackResult.Miss:
                damageTxt.text = "빗나감";
                break;
            case AttackResult.Evasion:
                damageTxt.text = "회피";
                break;
            case AttackResult.Normal:
                damageTxt.text = $"{damage}";
                break;
            case AttackResult.Heal:
                damageTxt.color = Color.green;
                damageTxt.text = $"{damage}";
                break;
        }

        Invoke("DeactiveDamage", duration);
    }

    void DeactiveDamage() => damageHUD.SetActive(false);

    public void ShowTurnEffect()
    {
        turnEffect.SetActive(true);

        Invoke("DeactiveTurn", 1f);
    }

    void DeactiveTurn() => turnEffect.SetActive(false);

    #region 마우스 이벤트
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 캐릭터 마우스 올리면 표시
        ground.DOColor(Color.black, 0f);

        if (owner.IsAlly)
            return;

        UIManager.GetInstance.SetEnemyToolTip(owner);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ground.DOColor(Color.white, 0f);
        UIManager.GetInstance.enemyTooltip.SetActive(false);
    }
    #endregion
}
