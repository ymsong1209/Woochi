using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyCard : MonoBehaviour
{
    [SerializeField] BaseCharacter ally;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI healthTxt;
    
    public void UpdateHP()
    {
        float currentHP = ally.Health.CurHealth;
        float maxHP = ally.Health.MaxHealth;

        if (currentHP <= 0) healthTxt.color = Color.gray;
        else healthTxt.color = Color.white;

        healthTxt.text = $"{currentHP}/{maxHP}";
    }

    public void Activate(BaseCharacter _ally)
    {
        ally = _ally;
        ally.onHealthChanged += UpdateHP;
        gameObject.SetActive(true);
    }

    public void Deactivate() 
    {
        ally = null;
        gameObject.SetActive(false);
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
