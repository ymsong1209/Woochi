using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 모든 기연의 Id, gameobject를 library에다가 입력후,
/// 이번 스테이지에서 출현하고 싶은 기연을 MapConfig에다 입력해주면됨.
/// 기연이 나올 확률도 MapConfig에서 조절
/// </summary>
public class StrangeManager : SingletonMonobehaviour<StrangeManager>
{
    public GameObject strangeObject;

    private Strange currentStrange;         // 현재 발생한 기연

    [Header("UI")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TextMeshProUGUI[] choiceTexts;
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private Button nextBtn;

    [Header("Status")]
    [HideInInspector] public bool isBattleStrange;        // 전투 기연인가?

    void Start()
    {
        strangeObject.gameObject.SetActive(false);

        #region Event
        for(int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            choiceButtons[i].onClick.AddListener(() =>
            {
                ShowResult(currentStrange.Select(index));
            });
        }
        nextBtn.onClick.AddListener(Next);
        #endregion
    }

    public void InitializeStrange(int strangeID)
    {
        isBattleStrange = false;

        Strange strange = GameManager.GetInstance.Library.GetStrange(strangeID);
        currentStrange = strange;
        
        if(strangeID >= 1000 && strangeID < 2000)
        {
            GameManager.GetInstance.soundManager.PlaySFX("Event_Positive");
        }
        else if (strangeID >= 2000 && strangeID < 3000)
        {
            GameManager.GetInstance.soundManager.PlaySFX("Event_Arrived");
        }
        else
        {
            GameManager.GetInstance.soundManager.PlaySFX("Event_Negative");
        }
        SetUI();
    }
    
    private void SetUI()
    {
        image.sprite = currentStrange.situationSprite;
        text.text = currentStrange.situationText;

        strangeObject.SetActive(true);
        choicePanel.SetActive(true);
        effectText.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);

        for(int i = 0; i < choiceButtons.Length; i++)
        {
            if(i < currentStrange.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceTexts[i].text = currentStrange.choices[i].text;
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void ShowResult(StrangeResult result)
    {
        if (result == null) return;

        choicePanel.SetActive(false);

        effectText.gameObject.SetActive(true);
        effectText.text = result.effect;

        image.sprite = result.sprite;
        text.text = result.text;

        nextBtn.gameObject.SetActive(true);
    }

    private void Next()
    {
        strangeObject.SetActive(false);

        if (isBattleStrange) return;

        MapManager.GetInstance.CompleteNode();
    }
}
