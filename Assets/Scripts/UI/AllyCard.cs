using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyCard : MonoBehaviour, ITooltipiable
{
    BaseCharacter ally = null;
    [SerializeField] Button front;
    [SerializeField] GameObject back;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI healthTxt;

    public Action<AllyCard, UIEvent> OnUIEvent;
    private bool isDead = false;
    private Vector3 originPos;
    
    private void Awake()
    {
        SetActivate(false);
        originPos = transform.localPosition;
    }

    private void Start()
    {
        front.onClick.AddListener(() => OnUIEvent?.Invoke(this, UIEvent.MouseClick));
    }

    public void UpdateHP()
    {
        float currentHP = ally.Health.CurHealth;
        float maxHP = ally.Health.MaxHealth;

        if (currentHP <= 0) Dead();
        else if(isDead && currentHP > 0)
        {
            SetActivate(true);
            healthTxt.color = Color.white;
            isDead = false;
        }

        healthTxt.text = $"{currentHP}/{maxHP}";
    }

    public void Show(BaseCharacter ally)
    {
        this.ally = ally;
        this.ally.onHealthChanged += UpdateHP;
        SetActivate(true);
        portrait.sprite = this.ally.Portrait;

        UpdateHP();
    }
    
    public void ShowAnimation(bool cursorOn)
    {
        if (cursorOn)
        {
            transform.DOLocalMoveY(originPos.y + 5, 0.1f).SetEase(Ease.OutQuad);
        }
        else
        {
            transform.DOLocalMoveY(originPos.y, 0.1f).SetEase(Ease.OutQuad);
        }
    }
    
    private void SetActivate(bool isActive)
    {
        front.gameObject.SetActive(isActive);
        portrait.gameObject.SetActive(isActive);
        back.SetActive(!isActive);
    }

    private void Dead()
    {
        healthTxt.color = Color.gray;
        isDead = true;
    }

    #region Getter
    public BaseCharacter Ally => ally;
    #endregion

    #region UI Event
    public void ShowTooltip() => OnUIEvent?.Invoke(this, UIEvent.MouseEnter);

    public void HideTooltip() => OnUIEvent?.Invoke(this, UIEvent.MouseExit);

    #endregion
}
