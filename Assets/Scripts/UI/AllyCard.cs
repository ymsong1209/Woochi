using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyCard : MonoBehaviour
{
    BaseCharacter ally = null;
    [SerializeField] GameObject front;
    [SerializeField] GameObject back;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI healthTxt;

    private bool isDead = false;

    private void Awake()
    {
        SetActivate(false);
    }

    public void UpdateHP()
    {
        float currentHP = ally.Health.CurHealth;
        float maxHP = ally.Health.MaxHealth;

        if (currentHP <= 0) Dead();
        else if(isDead && currentHP > 0)
        {
            SetActivate(true);
            isDead = false;
        }

        healthTxt.text = $"{currentHP}/{maxHP}";
    }

    public void Show(BaseCharacter _ally)
    {
        ally = _ally;
        ally.onHealthChanged += UpdateHP;
        SetActivate(true);
        portrait.sprite = ally.Portrait;

        UpdateHP();
    }

    private void SetActivate(bool isActive)
    {
        front.SetActive(isActive);
        portrait.gameObject.SetActive(isActive);
        back.SetActive(!isActive);
    }

    private void Dead()
    {
        SetActivate(false);
        isDead = true;
    }

    #region Getter
    public BaseCharacter Ally => ally;
    #endregion
}
