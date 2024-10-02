using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tool : MonoBehaviour, ITooltipiable, IPopupable
{
    public Action<Tool> OnShowTooltip;
    public Action OnHideTooltip;
    public Action<Tool> OnShowPopup;

    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI priceTxt;

    public string toolName;
    [SerializeField] protected string description;
    [SerializeField] protected string resultTxt;
    protected int price;

    private void Start()
    {
        btn.onClick.AddListener(Use);
        Price = 100;
    }

    public virtual void Use()
    {
        if (HelperUtilities.Buy(price))
        {
            Debug.Log($"{toolName}을 {price}만큼 지불해 사용합니다");
            Price *= 2;
            ShowPopup();
        }
        else
        {
            return;
        }
    }

    public virtual string GetDescription()
    {
        return description;
    }

    public virtual string GetResult()
    {
        return resultTxt;
    }

    public void ShowTooltip()
    {
        OnShowTooltip.Invoke(this);
    }

    public void HideTooltip()
    {
        OnHideTooltip.Invoke();
    }

    public void ShowPopup()
    {
        OnShowPopup.Invoke(this);
    }

    public int Price
    {
        get { return price; }
        set
        {
            price = value;
            priceTxt.text = price.ToString();
        }
    }
}
