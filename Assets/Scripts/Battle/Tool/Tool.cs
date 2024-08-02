using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI priceTxt;

    public string toolName;
    [SerializeField] protected string description;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.GetInstance.rewardToolPopup.ShowToolPopup(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.GetInstance.rewardToolPopup.HidePopup();
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
