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
    
    public void UpdateHP(BaseCharacter _ally)
    {
        float currentHP = _ally.Health.CurHealth;
        float maxHP = _ally.Health.MaxHealth;

        healthTxt.text = $"{currentHP}/{maxHP}";
    }

    public void Activate(BaseCharacter _ally)
    {
        ally = _ally;
        ally.Health.OnHealthChanged += UpdateHP;
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

        ally.Health.OnHealthChanged(ally);
    }
    #region Getter
    public BaseCharacter Ally => ally;
    #endregion
}
