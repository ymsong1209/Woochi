using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Strange_SanSam : BaseStrange
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button eatBtn;
    [SerializeField] private Image eatImage;
    [SerializeField] private Button ignoreBtn;
    [SerializeField] private Image ignoreImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI buffdescriptionText;
    
    // Start is called before the first frame update
    void Start()
    {
        continueBtn.onClick.AddListener(() => Deactivate());
        eatBtn.onClick.AddListener(() => Eat());
        ignoreBtn.onClick.AddListener(() => Ignore());
    }


    public override void Initialize()
    {
        base.Initialize();
        continueBtn.gameObject.SetActive(false);
        eatBtn.gameObject.SetActive(true);
        ignoreBtn.gameObject.SetActive(true);
        eatImage.gameObject.SetActive(false);
        ignoreImage.gameObject.SetActive(false);
        descriptionText.text = "연못 주변에서 운 좋게 오래 묵은 하수오를 발견했다!\n도움이 되지 않을까?";
        buffdescriptionText.gameObject.SetActive(false);
    }

    private void Eat()
    {
        descriptionText.text = "내 도력이 상승하는 것 같다! 먹길 잘 했다!";
        buffdescriptionText.gameObject.SetActive(true);
        eatImage.gameObject.SetActive(true);
        ContinueEvent();
    }

    private void Ignore()
    {
        descriptionText.text = "귀찮아서 무시했다.\n뭔가 후회된다.";
        ignoreImage.gameObject.SetActive(true);
        ContinueEvent();
    }

    private void ContinueEvent()
    {
        eatBtn.gameObject.SetActive(false);
        ignoreBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(true);
    }
}
