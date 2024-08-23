using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyCard : MonoBehaviour
{
    [SerializeField] BaseCharacter ally;
    [SerializeField] Image image;
    [SerializeField] Image portrait;
    [SerializeField] Button btn;
    [SerializeField] TextMeshProUGUI healthTxt;

    public void UpdateHP()
    {
        float currentHP = ally.Health.CurHealth;
        float maxHP = ally.Health.MaxHealth;

        if (currentHP <= 0) healthTxt.color = Color.gray;
        else healthTxt.color = Color.white;

        healthTxt.text = $"{currentHP}/{maxHP}";
    }

    public void Activate(BaseCharacter _ally, Sprite front)
    {
        ally = _ally;
        ally.onHealthChanged += UpdateHP;

        image.sprite = front;
        portrait.gameObject.SetActive(true);
        portrait.sprite = ally.Portrait;

        healthTxt.gameObject.SetActive(true);
    }

    public void Deactivate(Sprite back) 
    {
        ally = null;

        image.sprite = back;
        portrait.gameObject.SetActive(false);
        
        healthTxt.gameObject.SetActive(false);
    }

    public void SetInteractable(bool _able)
    {
        if (ally == null || ally.IsDead)
        {
            btn.interactable = false; 
            return;
        }

        btn.interactable = _able;
    }

    public void UpdateCard()
    {
        if (ally == null) return;

        ally.onHealthChanged?.Invoke();
    }
    #region Getter
    public BaseCharacter Ally => ally;
    #endregion
}
