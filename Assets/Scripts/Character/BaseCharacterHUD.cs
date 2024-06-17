using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class BaseCharacterHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private BaseCharacter owner;
    [SerializeField] private SpriteRenderer hpBar;
    [SerializeField] private SpriteRenderer ground;

    [Header("Damage HUD")]
    [SerializeField] private GameObject damageHUD;
    [SerializeField] private TextMeshPro damageTxt;

    [Header("Arrow")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject selectedArrow;

    bool isSelected = false;

    private void Awake()
    {
        owner = GetComponent<BaseCharacter>();
        owner.onHealthChanged += UpdateHPBar;
        owner.onAttacked += SetDamageText;
    }

    public void UpdateHPBar()
    {
        float curHealth = owner.Health.CurHealth;
        float maxHealth = owner.Health.MaxHealth;
        float nextScale = curHealth / maxHealth;
        hpBar.transform.DOScaleX(nextScale, 1f).SetEase(Ease.OutCubic);
    }

    public void SetDamageText(AttackResult _result, int damage = 0, bool isCrit = false)
    {
        damageHUD.SetActive(true);

        if (isCrit)
        {
            damageTxt.color = Color.red;
            damageTxt.fontSize = 15;
        }
        else
        {
            damageTxt.color = Color.white;
            damageTxt.fontSize = 10;
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
        }

        Invoke("DeactiveDamage", 1f);
    }

    void DeactiveDamage() => damageHUD.SetActive(false);

    public void ActivateArrow(bool isActive)
    {
        arrow.SetActive(isActive);
        selectedArrow.SetActive(false);
    }

    public void InitSelection() => isSelected = false;
    
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!BattleManager.GetInstance.canChangeLocation) return;

        if(isSelected)
        {
            selectedArrow.SetActive(false);
            isSelected = false;
        }
        else
        {
            selectedArrow.SetActive(true);
            isSelected = true;
        }

        BattleManager.GetInstance.CharacterSelected(owner);
    }
    #endregion
}
