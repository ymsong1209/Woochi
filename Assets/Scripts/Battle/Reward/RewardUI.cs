using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour, ITooltipiable
{
    public Action<RewardUI, UIEvent> OnUIEvent;

    [SerializeField] protected Image image;
    [SerializeField] protected Button btn;

    [Header("Reward")]
    [SerializeField] protected Reward reward;
    protected string popupText;

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
            popupText = reward.GetResult();
        }
        else
        {
            popupText = reward.GetError();
        }
        
        AkSoundEngine.PostEvent("Reward_Click", gameObject);
        OnUIEvent?.Invoke(this, UIEvent.MouseClick);
    }

    public void ShowTooltip()
    {
        AkSoundEngine.PostEvent("Reward_Mouse", gameObject);
        OnUIEvent?.Invoke(this, UIEvent.MouseEnter);
    }

    public void HideTooltip()
    {
        OnUIEvent?.Invoke(this, UIEvent.MouseExit);
    }

    public Reward GetReward() => reward;
    public string GetPopupText() => popupText;
}
