using TMPro;
using UnityEngine;

public class RewardToolPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private TextMeshProUGUI gradeTxt;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowRewardPopup(Reward reward)
    {
        gameObject.SetActive(true);

        nameTxt.text = reward.rewardName;
        descriptionTxt.text = reward.description;
        
        switch(reward.rarity)
        {
            case RareType.Lowest:
                gradeTxt.text = "����";
                break;
            case RareType.Low:
                gradeTxt.text = "��";
                break;
            case RareType.Middle:
                gradeTxt.text = "��";
                break;
            case RareType.High:
                gradeTxt.text = "��";
                break;
            case RareType.Highest:
                gradeTxt.text = "�ֻ�";
                break;
        }
    }   
    
    public void ShowToolPopup(Tool tool)
    {
        gameObject.SetActive(true);

        nameTxt.text = tool.toolName;
        descriptionTxt.text = tool.GetDescription();
        gradeTxt.text = "";
    }

    public void ShowText(string text)
    {
        nameTxt.text = "";  gradeTxt.text = "";
        descriptionTxt.text = text;
    }

    public void HidePopup()
    {
        gameObject.SetActive(false);
    }
}
