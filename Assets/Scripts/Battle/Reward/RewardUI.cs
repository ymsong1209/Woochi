using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour, ITooltipiable, IPopupable
{
    public Action<RewardUI> OnShowTooltip;
    public Action OnHideTooltip;
    public Action<string> OnShowPopup;

    [SerializeField] protected Image image;
    [SerializeField] protected Button btn;

    [Header("Reward")]
    [SerializeField] protected Reward reward;

    private void Start()
    {
        btn.onClick.AddListener(Receive);
    }

    public virtual void Initialize(Reward reward = null)
    {
        if (reward != null)
        {
            this.reward = reward;
            image.sprite = reward.sprite;
        }
    }

    /// <summary>
    /// 보상을 선택 가능하게 만들거나 불가능하게 하거나
    /// </summary>
    public void SetInteractable(bool active)
    {
        btn.interactable = active;
    }

    protected virtual void Receive()
    {
        if (reward.ApplyReward())
        {
            EventManager.GetInstance.onSelectReward?.Invoke(false);
            ShowPopup(reward.GetResult());
        }
        else
        {
            ShowPopup(reward.GetError());
        }
    }

    public void ShowTooltip()
    {
        OnShowTooltip.Invoke(this);
    }

    public void HideTooltip()
    {
        OnHideTooltip.Invoke();
    }

    public Reward GetReward() => reward;

    public void ShowPopup(string text)
    {
        OnShowPopup.Invoke(text);
    }
}
