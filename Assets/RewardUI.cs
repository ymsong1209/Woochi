using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour, IPointerEnterHandler
{
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

        // image.sprite = reward.sprite;
    }

    /// <summary>
    /// 보상을 선택 가능하게 만들거나 불가능하게 하거나
    /// </summary>
    public void SetInteractable(bool active)
    {
        btn.interactable = active;
    }

    private void Receive()
    {
        reward.ApplyReward();
        EventManager.GetInstance.onSelectReward?.Invoke(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(reward.rewardName);
    }
}
