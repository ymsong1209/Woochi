using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour, ITooltipiable, IPopupable
{
    public Action<RewardUI> OnShowTooltip;
    public Action OnHideTooltip;
    public Action<Reward> OnShowPopup;
    private Reward reward;

    [SerializeField] private Image image;
    [SerializeField] private Button btn;

    private void Start()
    {
        btn.onClick.AddListener(Receive);
    }

    public void Initialize(Reward reward)
    {
        this.reward = reward;

        image.sprite = reward.sprite;
    }

    /// <summary>
    /// ������ ���� �����ϰ� ����ų� �Ұ����ϰ� �ϰų�
    /// </summary>
    public void SetInteractable(bool active)
    {
        btn.interactable = active;
    }

    private void Receive()
    {
        if (reward.ApplyReward() == false) return;
        EventManager.GetInstance.onSelectReward?.Invoke(false);
        ShowPopup();
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

    public void ShowPopup()
    {
        OnShowPopup.Invoke(reward);
    }
}
