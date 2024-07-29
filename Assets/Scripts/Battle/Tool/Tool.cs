using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI priceTxt;

    [SerializeField] protected string toolName;
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

    protected virtual string GetDescription()
    {
        return description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(GetDescription());
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
