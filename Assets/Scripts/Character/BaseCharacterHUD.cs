using System;
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
    [SerializeField] private bool canInteract = true;

    private void Awake()
    {
        owner = GetComponent<BaseCharacter>();
        owner.onAttacked += SetDamageText;
    }

    private void Start()
    {
        hpBar?.SetOwner(owner);
        BattleManager.GetInstance.OnFocusStart += FocusStart;
        BattleManager.GetInstance.OnFocusEnd += () => canInteract = true;
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

    private void FocusStart()
    {
        canInteract = false;
        ground.DOColor(Color.white, 0f);
        UIManager.GetInstance.characterTooltip.SetActive(false);
    }

    #region 마우스 이벤트
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 캐릭터 마우스 올리면 표시
        if(!canInteract) return;
        ground.DOColor(Color.black, 0f);
        UIManager.GetInstance.SetCharacterToolTip(owner);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!canInteract) return;
        ground.DOColor(Color.white, 0f);
        UIManager.GetInstance.characterTooltip.SetActive(false);
    }
    #endregion
}
