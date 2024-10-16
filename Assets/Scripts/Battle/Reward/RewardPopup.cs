using TMPro;
using UnityEngine;

public class RewardPopup : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private GameObject infoPopup;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private TextMeshProUGUI gradeTxt;

    [Header("Result")]
    [SerializeField] private GameObject resultPopup;
    [SerializeField] private TextMeshProUGUI resultDescTxt;

    void Start()
    {
        infoPopup.SetActive(false);
        resultPopup.SetActive(false);
    }

    public void ShowTooltip(RewardUI rewardUI)
    {
        infoPopup.SetActive(true);
        SetTransform(rewardUI.transform);

        Reward reward = rewardUI.GetReward();
        nameTxt.text = reward.rewardName;
        descriptionTxt.text = reward.description;

        if (rewardUI is ToolUI)
        {
            gradeTxt.text = "";
        }
        else
        {
            switch (reward.rarity)
            {
                case RareType.Lowest:
                    gradeTxt.text = "최하";
                    break;
                case RareType.Low:
                    gradeTxt.text = "하";
                    break;
                case RareType.Middle:
                    gradeTxt.text = "중";
                    break;
                case RareType.High:
                    gradeTxt.text = "상";
                    break;
                case RareType.Highest:
                    gradeTxt.text = "최상";
                    break;
            }
        }
    }   

    public void ShowResult(string text)
    {
        infoPopup.SetActive(false);
        resultPopup.SetActive(true);

        resultDescTxt.text = text;
    }

    public void HideInfo()
    {
        infoPopup.SetActive(false);
    }

    private void SetTransform(Transform transform)
    {
        Vector3 pos = transform.position;
        infoPopup.transform.position = pos + new Vector3(0, -150, 0);
    }
}
