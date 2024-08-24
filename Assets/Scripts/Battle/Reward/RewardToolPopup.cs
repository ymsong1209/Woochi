using TMPro;
using UnityEngine;

public class RewardToolPopup : MonoBehaviour
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

    public void ShowRewardPopup(Reward reward)
    {
        infoPopup.SetActive(true);
        SetTransform();

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
        infoPopup.SetActive(true);
        SetTransform();

        nameTxt.text = tool.toolName;
        descriptionTxt.text = tool.GetDescription();
        gradeTxt.text = "";
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

    private void SetTransform()
    {
        Vector2 pos = Input.mousePosition;
        pos.y -= 100;
        infoPopup.transform.position = pos;
    }
}
