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
        // ������ �� �ִ��� Ȯ�� �� ���� ����
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
            string text = "�帧�� �����մϴ�";
            ShowPopup(text);
        }
    }

    protected void SetPrice(int newPrice)
    {
        price = newPrice;
        priceTxt.text = price.ToString();
    }
}
