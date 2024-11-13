using TMPro;
using UnityEngine;

public class ToolUI : RewardUI
{
    [Header("Price")]
    [SerializeField] protected TextMeshProUGUI priceTxt;
    [SerializeField] protected float priceMultiplier = 1f;
    [SerializeField] protected int basePrice = 100;
    protected int price;

    void Start()
    {
        if(reward != null)
        {
            btn.onClick.AddListener(Receive);
            image.sprite = reward.sprite;
        }
    }

    public override void Initialize(Reward reward = null)
    {
        SetPrice(basePrice);
    }

    protected override void Receive()
    {
        // 구매할 수 있는지 확인 후 보상 수령
        if (HelperUtilities.Buy(price))
        {
            if(reward.ApplyReward())
            {
                SetPrice(Mathf.RoundToInt(price * priceMultiplier));
                ShowPopup(reward.GetResult());
            }
            else
            {
                HelperUtilities.AddGold(price);
                ShowPopup(reward.GetError());
            }
        }
        else
        {
            string text = "흐름이 부족합니다";
            ShowPopup(text);
        }
    }

    protected void SetPrice(int newPrice)
    {
        price = newPrice;
        priceTxt.text = price.ToString();
    }
}
