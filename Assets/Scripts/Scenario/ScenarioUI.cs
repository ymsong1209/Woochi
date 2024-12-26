using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioUI : MonoBehaviour
{
    [SerializeField] private GameObject scenarioPanel;
    [SerializeField] private GameObject upperPanel;
    [SerializeField] private GameObject lowerPanel;
    [SerializeField] private GameObject backgroundPanel;
    
    [Header("Illustration")]
    [SerializeField] private GameObject illustrationPanel;
    [SerializeField] private Image[] illustrations;
    
    [Header("Text")]
    [SerializeField] private GameObject textPanel;
    [SerializeField] private TextMeshProUGUI text;
    
    [Header("Next")]
    [SerializeField] private Button nextBtn;
    
    [Header("Pos")]
    [SerializeField] private Transform[] textPos;

    [Header("Blind")] 
    [SerializeField] private Image blind;
    
    private Plot currentPlot;
    
    void Start()
    {
        nextBtn.onClick.AddListener(Click);
        blind.alphaHitTestMinimumThreshold = 0.1f;
        
        BattleManager.GetInstance.OnFocusStart += () => blind.gameObject.SetActive(false);
    }

    public void SetUI(Plot plot)
    {
        currentPlot = plot;

        SetPanel();
        SetIllustration();
        SetText();
    }
    
    public void ActivateUI(bool isShow)
    {
        scenarioPanel.SetActive(isShow);
    }

    private void SetPanel()
    {
        bool showBattle = currentPlot.showBattle;

        upperPanel.SetActive(!showBattle);
        lowerPanel.SetActive(!showBattle);
        backgroundPanel.SetActive(!showBattle);
        
        bool isTextUp = currentPlot.isTextUp;
        textPanel.transform.position = textPos[isTextUp ? 0 : 1].position;
        
        bool showText = currentPlot.showText;
        textPanel.SetActive(showText);
        
        bool isBlind = currentPlot.isBlind;
        blind.gameObject.SetActive(isBlind);
        if (currentPlot.isBlind)
        {
            blind.sprite = currentPlot.blindImage;
        }
        
        bool isEvent = currentPlot.plotEvent != PlotEvent.None;
        nextBtn.gameObject.SetActive(!isEvent);
        blind.alphaHitTestMinimumThreshold = isEvent ? 0.1f : 0f;
    }
    
    private void SetIllustration()
    {
        bool isShow = currentPlot.illustrations.Length > 0;
        illustrationPanel.SetActive(isShow);
        
        for (int i = 0; i < illustrations.Length; i++)
        {
            illustrations[i].gameObject.SetActive(false);
        }
        
        if (isShow)
        {
            for(int i = 0; i < currentPlot.illustrations.Length; i++)
            {
                int direction = currentPlot.illustrations[i].direction;
                illustrations[direction].sprite = currentPlot.illustrations[i].actor.actorImage;
                illustrations[direction].SetNativeSize();
                illustrations[direction].gameObject.SetActive(true);
            }
        }
    }

    private void SetText()
    {
        text.text = string.Empty;
        
        string result = string.Empty;
        string colorCode = string.Empty;
        if (currentPlot.type == ScenarioType.Narration)
        {
            colorCode = "#FFFFFF";
            text.lineSpacing = 42f;
        }
        else if (currentPlot.type == ScenarioType.Dialogue)
        {
            colorCode = "#CABEB4";
            text.lineSpacing = 64f;
            result = $"[{currentPlot.speaker.actorName}]\n";
        }
        else if (currentPlot.type == ScenarioType.Guide)
        {
            colorCode = "#E4C36A";
            text.lineSpacing = 64f;
            result = $"[{currentPlot.speaker.actorName}]\n";
        }
        
        Color newColor;
        ColorUtility.TryParseHtmlString(colorCode, out newColor);
        text.color = newColor;
        
        result += currentPlot.text;
        text.text = result;
    }
    
    private void Click()
    {
        currentPlot.Next(PlotEvent.None);
    }
}
