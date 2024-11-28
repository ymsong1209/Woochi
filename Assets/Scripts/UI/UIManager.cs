using UnityEngine;
using TMPro;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [HeaderTooltip("Canvas", "UI Canvas")] 
    [SerializeField] private Canvas canvas;
    private RectTransform canvasRt;
    
    [HeaderTooltip("HP tooltip", "체력바 툴팁")]
    [SerializeField] GameObject hpTooltip;
    [SerializeField] TextMeshProUGUI hpTooltipText;

    [HeaderTooltip("Enemy Tooltip", "적 캐릭터에 마우스 올릴 시 적 정보 뜨는 툴팁")]
    public GameObject enemyTooltip;
    [SerializeField] private TextMeshProUGUI enemyNameTxt;
    [SerializeField] private TextMeshProUGUI enemyEvasionTxt;
    [SerializeField] private TextMeshProUGUI enemySpeedTxt;
    [Space]
    [SerializeField] private AllyCharacterUI allyCharacterUI;
    [SerializeField] private BuffPopupUI buffPopupUI;

    [HeaderTooltip("Woochi", "우치 전용 UI")]
    public SorceryGuageUI sorceryGuageUI;
    [SerializeField] private WoochiActionList woochiActionList;

    [HeaderTooltip("Battle Result", "전투 결과")]
    public LevelUpUI levelUpUI;
    
    public RecruitUI recruitUI; 
    
    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += DeactivateBuffPopUp;
        canvasRt = canvas.GetComponent<RectTransform>();
    }
    
    public void SetHPTooltip(bool isActivate, HPBar hpBar = null)
    {
        hpTooltip.SetActive(isActivate);

        if(hpBar)
        {
            RectTransform rt = hpTooltip.GetComponent<RectTransform>();
            rt.position = hpBar.GetPosition();
            hpTooltipText.text = hpBar.GetTooltipText();
        }
    }

    public void SetEnemyToolTip(BaseCharacter _character)
    {
        enemyTooltip.SetActive(true);
        enemyNameTxt.text = _character.Name;
        
        Stat finalStat = _character.FinalStat;
        enemyEvasionTxt.text = $"회피 : {finalStat.GetValue(StatType.Evasion)}";
        enemySpeedTxt.text = $"속도 : {finalStat.GetValue(StatType.Speed)}";
    }

    public void SetTooltipPosition(RectTransform targetRt, RectTransform tooltipRt, Vector2 offset)
    {
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(null, targetRt.position);

        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRt, screenPosition,
            null, out localPosition);
        
        tooltipRt.localPosition = localPosition + offset;
    }

    public void DeactivePopup()
    {
        enemyTooltip.SetActive(false);
        buffPopupUI.Deactivate();
    }
    
    #region BuffPopupUI
    public BuffPopupUI BuffPopupUI => buffPopupUI;
    public void ActivateBuffPopUp(BuffIcon icon)
    {
        buffPopupUI.Activate(icon);
    }
    
    public void DeactivateBuffPopUp()
    {
        buffPopupUI.Deactivate();
    }
    #endregion
}
